using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;

namespace Test.Dialogs
{
    public class CheckUserDialog : ComponentDialog
    {
        public static int PrimaryNumber = 0;
        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        public CheckUserDialog() : base(nameof(CheckUserDialog))
        {
            AddDialog(new TutorialDialog()); //튜토리얼 다이얼로그 추가
            AddDialog(new ShowFunctionsDialog()); //기능보여주기 다이얼로그 추가

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                AskVisitedAsync, //이전에 방문했던 사용자인지 물어봄
                AskNameAsync, //이전에 방문했던 사용자라면 이름 물어봄
                CheckValidUserAsync, //DB에서 사용자 있는지 찾아봄
                EndCheckUserAsync //이 다이얼로그 종료
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> AskVisitedAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"이전에 Healthee를 이용하신 적이 있나요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "응", "아니" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string visitedbefore = ((FoundChoice)stepContext.Result).Value.Trim();

            if (visitedbefore.Equals("응")) //이전에 방문한 적이 있다고 함
            {
                var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("이름을 알려주세요!") };
                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
            }
            else { //Tutorial을 진행해야함
                ModeManager.mode = (int)ModeManager.Modes.Tutorial;
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 1;
                //return await stepContext.NextAsync();
                return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);
            }
        }

        private static async Task<DialogTurnResult> CheckValidUserAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//0
            var username = (string)stepContext.Result;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[UserInfo] WHERE UserName='" + username + "'"); //유저가 입력한 이름으로 DB에서 사용자 찾기
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

                    sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[UserInfo] ");
                    sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PrimaryNumber = reader.GetInt32(0)+1;
                            }
                        }
                    }

                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("count : "+ count), cancellationToken);

                    
                    if (count == 0) //DB에 기록이 없을 때, tutorial Async 실행시켜야 함
                    { 
                        connection.Close();
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("CheckUserDialog - Tutorial을 진행합니다."), cancellationToken);
                        ModeManager.mode = (int)ModeManager.Modes.Tutorial;
                        return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);

                    }
                    else //DB에 기록이 있으면 기능 보여주기로 넘어감
                    {
                        sb = new StringBuilder();
                        sb.Append("SELECT * FROM [dbo].[UserInfo] WHERE UserName='" + username + "'");
                        sql = sb.ToString();

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserInfoManager.UserName = (string)reader.GetValue(1);
                                    UserInfoManager.PreWeight = (int)reader.GetValue(2);
                                    UserInfoManager.PostWeight = (int)reader.GetValue(3);
                                    UserInfoManager.SkillLevel = (string)reader.GetValue(4);
                                    UserInfoManager.Area = (string)reader.GetValue(5);
                                    UserInfoManager.Category = (string)reader.GetValue(6);
                                    UserInfoManager.ConversationCount = (int)reader.GetValue(7);
                                    break;
                                }
                            }
                        }

                        connection.Close();
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("CheckUserDialog - ShowFunctions을 진행합니다."), cancellationToken);
                        ModeManager.mode = (int)ModeManager.Modes.ShowFunction;
                        return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
                    }

                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("문제가 있습니다."), cancellationToken);
                Console.WriteLine(e.ToString());
                return await stepContext.NextAsync();
            }
        }

        private static async Task<DialogTurnResult> EndCheckUserAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {//1
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("CheckUserDialog 종료합니다."), cancellationToken);
            return await stepContext.EndDialogAsync();
        }
    }
}
