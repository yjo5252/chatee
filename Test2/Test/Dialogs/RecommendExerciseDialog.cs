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

namespace Test.Dialogs
{
    public class RecommendExerciseDialog : ComponentDialog
    {

        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        private string Area;
        private string Kind;
        private string Level;


        public RecommendExerciseDialog() : base(nameof(RecommendExerciseDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                AskUserAsync, //맞춤운동 추천할지 물어보기
                CheckAnswerAsync, //맞춤운동 추천할지 확인하기
                SelectAreaAsync, //부위 묻기 
                SelectKindAsync, //종류
                SelectLevelAsync, //난이도
                ShowExerciseAsync,
                DidExerciseAsync,
                ShowResultStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult>AskUserAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //변수들 초기화
            Area = "";
            Kind = "";
            Level = "";

            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(UserInfoManager.UserName), cancellationToken);

            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("REFirst.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($""),
                Choices = ChoiceFactory.ToChoices(new List<string> { "응", "아니" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckAnswerAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            string check = ((FoundChoice)stepContext.Result).Value;

            if (check.Equals("응")) //맞춤 운동 추천해야함
            {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 3;
                return await stepContext.NextAsync(); 
            }
            else { //물어봐야 함
                return await stepContext.NextAsync(); //다음 단계로 진행
            }
        }

        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//0
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"운동하고 싶은 부위를 선택해주세요!"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "다리, 힙", "팔", "어깨 및 등", "가슴" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectKindAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//1
            Area = ((FoundChoice)stepContext.Result).Value; //부위

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"유산소, 근력, 스트레칭 중 어떤 운동을 추천해 드릴까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "유산소", "근력", "스트레칭" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectLevelAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//2
            Kind = ((FoundChoice)stepContext.Result).Value; //종류

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"난이도는 어느 정도이면 좋을까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "유산소", "근력", "스트레칭" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//3
            if(stepContext.Result != null)
                Level = ((FoundChoice)stepContext.Result).Value; //난이도

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Debugpoint1"), cancellationToken);

            string sportsName = "";
            string videourl = "";
            string explanation = "";
            string time = "";
            int set = 0;

            //DB 서치
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    if (Area.Equals("")) //맞춤 운동을 설정했을 경우 값을 사용자 정보에서 가져옴
                    {
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Debugpoint2"), cancellationToken);
                        Area = UserInfoManager.Area;
                        Kind = UserInfoManager.Category;
                        Level = UserInfoManager.SkillLevel;
                    }

                    //사용자가 설정한 정보에 해당하는 데이터 개수 세기

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[Sports] WHERE Category='"+Kind+"' and Area='"+Area+"' and Level='"+Level+"'"); //유저가 입력한 이름으로 DB에서 운동 개수 찾기
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
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Debugpoint3"), cancellationToken);

                    //DB에서 운동 정보 가져오기

                    sb = new StringBuilder();
                    sb.Append("SELECT * FROM [dbo].[Sports] WHERE Category='" + Kind + "' and Area='" + Area + "' and Level='" + Level + "'"); //유저가 입력한 이름으로 DB에서 운동 찾기
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
                                sportsName = (string)reader.GetValue(1);
                                videourl = (string)reader.GetValue(5);
                                explanation = (string)reader.GetValue(6);
                                time = (string)reader.GetValue(7);
                                set = (int)reader.GetValue(8);
                                if (index++ == randomnumber) break;
                            }
                        }
                    }

                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Debugpoint3"), cancellationToken);
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("문제가 있습니다."), cancellationToken);
                Console.WriteLine(e.ToString());
            }

            //카드 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var videoCard = new VideoCard
            {
                Title = sportsName,
                Subtitle = "by the Blender Institute",
                Text = "Big Buck Bunny (code-named Peach) is a short computer-animated comedy film by the Blender Institute,",
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = videourl,
                    },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "imBack",
                        Type = ActionTypes.ImBack,
                        Value = "확인했어!",
                    },
                },
            };
            reply.Attachments.Add(videoCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("") };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> DidExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//4
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"추천해드린 운동을 하셨나요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "응", "아니" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowResultStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//5
            var msg = "끝입니다.";
           
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.

            return await stepContext.EndDialogAsync();

        }
    }
}
