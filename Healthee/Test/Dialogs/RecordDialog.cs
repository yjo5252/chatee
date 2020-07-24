using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Test.Dialogs
{
    public class RecordDialog : ComponentDialog
    {
        public string sportName = "달리기";

        private string Area;
        private int Time;

        public RecordDialog() : base(nameof(RecordDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), TimePromptValidatorAsync));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialRecordAsync, //운동 기록하겠습니다. 보여줌
                CheckAreaAsync,//어디 부위를 운동하셨나요?
                CheckTimeAsync, //얼마나 운동하셨나요?
                RecordAsync, //운동 
                EndAsync 
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> InitialRecordAsync (WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //변수 초기화
            Area = "";
            Time = 0;

            //운동 기록할거라는 json 카드 보여줘야 함.
            //처음 카드 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("RecordInitial.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            Thread.Sleep(1500);

            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> CheckAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { 
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어떤 부위를 운동하셨나요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "다리, 힙", "팔", "어깨 및 등", "가슴" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

       
        private async Task<DialogTurnResult> CheckTimeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Area = ((FoundChoice)stepContext.Result).Value; //부위 설정

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"몇 분 동안 운동하셨나요?"),
                RetryPrompt = MessageFactory.Text("0보다 큰 값을 입력해주세요!"),
            };
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RecordAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Time = ((int)stepContext.Result) * 60; //시간 설정(초단위로 집어넣는다)
            UserInfoManager.ConversationCount += 1; //기록 횟수 + 1

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    //기록 DB에 사용자가 있는지 찾는다.
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기
                    String sql = sb.ToString();

                    int count = 0;

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

                    SqlCommand q;
                    string query;

                    if (count == 0) //기록이 없을 때
                    {
                        query = "INSERT INTO [dbo].[ExerciseRecord] VALUES(" + UserInfoManager.keyNum + ", 0, 0, 0, 0, 0);"; //사용자 기록을 만든다.
                        q = new SqlCommand(query, connection);
                        q.ExecuteNonQuery();
                    }

                    string area=""; //DB에서 찾을 부위 string 값
                    int index = 0; //DB에서 찾을 부분 index 값

                    if (Area.Equals("복근")) { area = "Abs"; index = 1; }
                    else if (Area.Equals("다리, 힙")) { area = "Leg"; index = 2; }
                    else if (Area.Equals("팔")) { area = "Arm"; index = 3; }
                    else if (Area.Equals("어깨 및 등")) { area = "Shoulder"; index = 4; }
                    else if (Area.Equals("가슴")) { area = "Chest"; index = 5; }

                    //이전 기록 읽어옴
                    sb = new StringBuilder();
                    sb.Append("SELECT * FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기
                    sql = sb.ToString();
                    

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Time += reader.GetInt32(index);
                            }
                        }
                    }

                    //사용자 운동 기록 업데이트
                    query = "UPDATE [dbo].[ExerciseRecord] SET ["+area+"]="+Time+" WHERE UserID="+UserInfoManager.keyNum; //사용자 기록 업데이트
                    q = new SqlCommand(query, connection);
                    q.ExecuteNonQuery();

                    //사용자 정보 업데이트
                    query = "UPDATE [dbo].[UserInfo] SET [ConversationCount]=" + UserInfoManager.ConversationCount + " WHERE UserID=" + UserInfoManager.keyNum; //사용자 정보 업데이트
                    q = new SqlCommand(query, connection);
                    q.ExecuteNonQuery();

                    connection.Close();

                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동 /예외 발생"));

            }

            //기록되었다는 거 알려줌 herocard/Adaptivecard 필요

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("기록되었습니다."), cancellationToken);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기록 보여주기 모드로 전환함.
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }

        private static Task<bool> TimePromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            //0보다 큰 값을 입력하도록 해야 한다.
            return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0);
        }

    }
}
