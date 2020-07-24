using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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

        // Define value names for values tracked inside the dialogs.
        private const string Record = "value-record";
        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        public RecordDialog() : base(nameof(RecordDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), TimePromptValidatorAsync));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                //AskUserWhatExerciseAsync,
                CheckExerciseAsync,
                SelectAreaAsync,
                SelectExerciseAsync,
                AskExerciseMinuteAsync,
                SummaryAsync

            }));
            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> AskUserWhatExerciseAsync (WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions { 
                Prompt = MessageFactory.Text("오늘 운동을 하셨나요?") ,
                RetryPrompt = MessageFactory.Text("응이라고 입력해주세요.")
            };

            // Ask the user to enter their name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
          // Create an object in which to collect the user's information within the dialog.
            stepContext.Values[Record]= new Record();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"구체적으로 기록을 남겨볼까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "기록할래" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

       
        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var record = (Record)stepContext.Values[Record];
            record.startRecord = (string)stepContext.Result.ToString().Trim(); //기록 시작


            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어디를 운동하셨나요??"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근","가슴", "팔", "다리" ,"어깨 등","힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var record = (Record)stepContext.Values[Record];
            record.Area = ((FoundChoice)stepContext.Result).Value; //목표 부위에 할당

            String[] str = { };
            List<String> list = new List<String>();
            var choice = (FoundChoice)stepContext.Result; //운동 부위
            var areaName = choice.Value.ToString();
            int count = 0;


            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * ");
                    sb.Append("FROM [dbo].[Sports] pc ");
                    sb.Append("WHERE [Area]='" + "복근" + "'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                sportName = reader.GetString(1);
                                list.Add("\""+sportName+"\"");
                                ///Outline = reader.GetString(3);
                                // await stepContext.Context.SendActivityAsync(MessageFactory.Text(reader.GetString(1)));

                                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(sportName));

                            }
                        }
                    }
                    connection.Close();

                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동 /예외 발생"));

            }


            int randomIndex = 0;

            str = list.ToArray();
            // { "복근","가슴", "팔", "다리" ,"어깨 등","힙" }
            string options = "{ ";
            string joined = string.Join(", ", list);

            /*
            foreach (string element in list)
            {

                joined = string.Join(", ", element);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("element:"+element));
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("joined string:"+joined));
            }
            */
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("joined string:" + joined));
            options = options+joined+ " }";
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("options:"+options));


            sportName = (string)str[randomIndex];
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text("string[]"+str)); //string[]System.String[]
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text("list<String>"+list)); //list<String>System.Collections.Generic.List`1[System.String]
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(options));




            //await stepContext.Context.SendActivityAsync(MessageFactory.Text($"그에 해당하는 운동이라면 이건가요? " + randomIndex ));


            // 수정 바람.#1
            /*  foreach (var item in list)
              {
                  await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동: " + (string)item ));

              }
              await stepContext.Context.SendActivityAsync(str.ToString());
            */
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어떤 운동을 하셨나요??"),
                //Choices = ChoiceFactory.ToChoices(new List<string> { "하이 플랭크", "다리 들어올리기" }),
                 Choices = ChoiceFactory.ToChoices(new List<string> { options })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            //return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> AskExerciseMinuteAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var record = (Record)stepContext.Values[Record];
            record.SportName = ((FoundChoice)stepContext.Result).Value; // 운동 이름에 할당



            // User said "yes" so we will be prompting for the age.
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("운동을 얼마나 했는지 입력해주세요!(단위 : 분)"),
                RetryPrompt = MessageFactory.Text("1이상의 값을 입력해주세요."),
            };
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SummaryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(("summary 시작")));
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            var record = (Record)stepContext.Values[Record];
            record.ExerciseHour = (int)stepContext.Result;  // 운동 시간에 할당

            //결과 보여줘야 함

            var msg = $"{userProfile.UserName}님!\nHealthee와 {record.Area}를 위해 {record.SportName} 운동을 { record.ExerciseHour } 했어요!!\n"
                        + $"{ userProfile.UserName}님의 캐릭터 { userProfile.AvatarName} 변화도 눈여겨봐주세요!";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("msg 작성함"));
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            // insert 
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    string query1 = "INSERT INTO [dbo].[ExerciseRecord] VALUES( " + CheckUserDialog.PrimaryNumber + ", '" + record.ExerciseHour + "', " + record.Area + ", " + record.SportName + ", '" + 0 + ");";
                 
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.ExecuteNonQuery();

                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("문제가 있습니다."), cancellationToken);
                Console.WriteLine(e.ToString());
            }

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
           // return await stepContext.EndDialogAsync();

            
            // Exit the dialog, returning the collected user information.
            return await stepContext.EndDialogAsync(stepContext.Values[Record], cancellationToken);

        }

        private static Task<bool> TimePromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            // This condition is our validation rule. You can also change the value at this point.
            return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0);
        }

    }
}
