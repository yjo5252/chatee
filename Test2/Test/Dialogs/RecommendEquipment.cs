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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Http;
using System;

namespace Test.Dialogs
{
    public class RecommendEquipment : ComponentDialog
    {
        public string EquipmentName = "덤벨";
        public string Videolink = "url";
        public string Salelink = "url";
        public string ImageUrl = "url";
        public RecommendEquipment() : base(nameof(RecommendEquipment))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                SelectAreaAsync, //어떤 부위
                RecommendResultAsync //추천
            }));

            InitialDialogId = nameof(WaterfallDialog);
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
        { //-1
            String[] str = { };
            List<String> list = new List<String>();

            var choice = (FoundChoice)stepContext.Result;
            var Area = choice.Value.ToString();
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(Area));
            string status = "ok";

            await stepContext.Context.SendActivityAsync(status);

            // hero card 생성
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            


            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
               
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                   connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * ");
                    sb.Append("FROM [dbo].[Equipment] pc ");
                    sb.Append("WHERE [Area]='" + Area + "'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetString(0));

                            }
                        }
                    }

                    // 선택한 부위에 해당하는 기구 중 랜덤하게 하나 선택 
                    Random rand = new Random();
                    int randomIndex = 0;
                    str = list.ToArray();
                    randomIndex = rand.Next(str.Length);
                    EquipmentName = (string)str[randomIndex];

                    StringBuilder sb2 = new StringBuilder();
                    sb2.Append("SELECT * ");
                    sb2.Append("FROM [dbo].[Equipment] pc ");
                    sb2.Append("WHERE [Area]='" + Area + "'");
                    sb2.Append(" [Name]='" + EquipmentName + "'");

                    // DB 에 접속해서 데이터 읽어오기 
                    String sql2 = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // list.Add(reader.GetString(0));
                                EquipmentName = reader.GetString(0);
                                Videolink = reader.GetString(2);
                                Salelink = reader.GetString(3);
                                ImageUrl = reader.GetString(4);

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"기구 카드 /예외 발생"));
                
            }


            var heroCard = new HeroCard
            {
                Title = "####  " + EquipmentName,
                Images = new List<CardImage> { new CardImage(ImageUrl) },
                Buttons = new List<CardAction> 
                {   
                    new CardAction(ActionTypes.PlayVideo, "기구 사용법 ", value: Videolink), 
                    new CardAction(ActionTypes.OpenUrl, "구매 링크 ", value: Salelink ) 
                },

            };
            reply.Attachments.Add(heroCard.ToAttachment());


            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);


            return await stepContext.EndDialogAsync();
        }


    }
}
