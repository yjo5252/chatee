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
    public class RecommendExerciseDialog : ComponentDialog
    {

        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        private string Area;
        private string Kind;
        private string Level;

        private string sportsName;
        private string videourl;
        private string explanation;
        private string time;
        private int set;


        public RecommendExerciseDialog() : base(nameof(RecommendExerciseDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                AskUserAsync, //맞춤운동 추천할지 물어보기
                CheckAnswerAsync, //맞춤운동 추천할지 확인하기
                SelectAreaAsync, //부위 묻기 
                SelectKindAsync, //종류
                SelectLevelAsync, //난이도
                ShowExerciseAsync, //운동 보여주기
                DidExerciseAsync, //운동을 했는지 확인
                ShowResultStepAsync, //결과
                EndAsync //끝
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult>AskUserAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //변수들 초기화
            Area = "";
            Kind = "";
            Level = "";

            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("REInitial.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            Thread.Sleep(1500);

            attachments = new List<Attachment>();
            reply = MessageFactory.Attachment(attachments);
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
                Choices = ChoiceFactory.ToChoices(new List<string> { "상", "중", "하" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//3
            if(stepContext.Result != null)
                Level = ((FoundChoice)stepContext.Result).Value; //난이도

            sportsName = "";
            videourl = "";
            explanation = "";
            time = "";
            set = 0;

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
                        Area = UserInfoManager.Area;
                        Kind = UserInfoManager.Category;
                        Level = UserInfoManager.SkillLevel;
                    }

                    if (Kind.Equals("스트레칭")) Level = "하"; //스트레칭은 레벨이 무조건 하이다.

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

                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("DB 연결에 문제가 있습니다."), cancellationToken);
                Console.WriteLine(e.ToString());
            }

            //카드 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var videoCard = new VideoCard
            {
                Title = sportsName,
                Subtitle = time+", "+set+"세트 진행하세요.",
                Text = explanation,
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
                        Title = "확인했어!",
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
            string check = ((FoundChoice)stepContext.Result).Value;

            if (check.Equals("응")) //기록 DB에 업데이트 해야 함.
            {
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

                        string area = ""; //DB에서 찾을 부위 string 값
                        int index = 0; //DB에서 찾을 부분 index 값
                        int Time = 0; //새로 업데이트 할 시간

                        if (Area.Equals("복근")) { area = "Abs"; index = 1; }
                        else if (Area.Equals("다리, 힙")) { area = "Leg"; index = 2; }
                        else if (Area.Equals("팔")) { area = "Arm"; index = 3; }
                        else if (Area.Equals("어깨 및 등")) { area = "Shoulder"; index = 4; }
                        else if (Area.Equals("가슴")) { area = "Chest"; index = 5; }

                        //이전 기록 읽어옴
                        sb = new StringBuilder();
                        sb.Append("SELECT * FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기
                        sql = sb.ToString();

                        if (time[time.Length - 1] == '회') { //몇 회로 데이터베이스에 저장된 경우
                            Time = 60; //1분으로 기록(초단위)
                        }
                        else { //몇 초라고 데이터베이스에 저장된 경우
                            Time = Convert.ToInt32(time.Substring(0, time.Length - 2)); //DB에 저장된 초를 가져온다.
                        }

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Time += reader.GetInt32(index); //시간 가져오기
                                }
                            }
                        }

                        //사용자 기록 업데이트
                        query = "UPDATE [dbo].[ExerciseRecord] SET [" + area + "]=" + Time + " WHERE UserID=" + UserInfoManager.keyNum; //사용자 기록 업데이트
                        q = new SqlCommand(query, connection);
                        q.ExecuteNonQuery();

                        connection.Close();

                    }
                }
                catch (SqlException e)
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동 /예외 발생"));

                }

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("기록되었습니다."), cancellationToken);
            }

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보여주기 모드로 바꾼다.
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);

        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
