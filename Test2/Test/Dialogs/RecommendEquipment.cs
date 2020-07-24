using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System.Data.SqlClient;
using System.Text;
using System;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Test.Dialogs
{
    public class RecommendEquipment : ComponentDialog
    {
        private string Area;

        private string EquipmentName;
        private string Videolink;
        private string Salelink;
        private string ImageUrl;

        public RecommendEquipment() : base(nameof(RecommendEquipment))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialAsync, //카드 보여주는 부분
                //CheckAnswerAsync,
                SelectAreaAsync, //어떤 부위인지 물어보기
                RecommendResultAsync, //추천
                EndAsync //종료
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //변수 초기화
            Area = "";
            EquipmentName = "";
            Videolink = "";
            Salelink = "";
            ImageUrl = "";

            //처음 카드 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("EquipmentInitial.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            Thread.Sleep(1500);

            return await stepContext.NextAsync();

        }


        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어떤 부위를 발달시키는 운동기구를 추천해드릴까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "전신", "가슴", "팔", "다리, 힙", "어깨 및 등" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RecommendResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { 
            Area = ((FoundChoice)stepContext.Result).Value;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    //사용자가 설정한 정보에 해당하는 데이터 개수 세기
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[Equipment] WHERE [Area]='" + Area + "'"); //유저가 입력한 이름으로 DB에서 운동 기구 개수 찾기
                    String sql = sb.ToString();

                    int count = 0; //사용자가 설정한 정보에 해당하는 데이터의 개수


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                count = reader.GetInt32(0);
                            }
                        }
                    }

                    //DB에서 운동 기구 정보 가져오기
                    sb = new StringBuilder();
                    sb.Append("SELECT * FROM [dbo].[Equipment] WHERE Area='" + Area + "'"); //유저가 입력한 이름으로 DB에서 운동 기구 찾기
                    sql = sb.ToString();

                    Random r = new Random();
                    int randomnumber = r.Next(1, count + 1);
                    int index = 0;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EquipmentName = (string)reader.GetValue(0);
                                Videolink = (string)reader.GetValue(2);
                                Salelink = (string)reader.GetValue(3);
                                ImageUrl = (string)reader.GetValue(4);
                                if (index++ == randomnumber) break;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"기구 카드 /예외 발생"));
                
            }

            // hero card 생성
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var heroCard = new HeroCard
            {
                Title = "####  " + EquipmentName,
                Images = new List<CardImage> { new CardImage(ImageUrl) },
                Buttons = new List<CardAction> 
                {   
                    new CardAction(ActionTypes.OpenUrl, "구매 링크 ", value: Salelink ) 
                },

            };
            reply.Attachments.Add(heroCard.ToAttachment());

            var videoCard = new VideoCard
            {
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = Videolink,
                    },
                }
            };
            reply.Attachments.Add(videoCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);


            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }


        }
}
