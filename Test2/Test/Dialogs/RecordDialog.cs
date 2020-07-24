using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
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

           
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"구체적으로 기록을 남겨볼까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "기록할래" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

       
        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어디를 운동하셨나요??"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근","가슴", "팔", "다리" ,"어깨 등","힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            String[] str = { };
            List<String> list = new List<String>();
            var choice = (FoundChoice)stepContext.Result;
            var areaName = choice.Value.ToString();
            

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {                    
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * ");
                    sb.Append("FROM [dbo].[Sports]  ");
                    sb.Append("WHERE [Area] = "+ areaName );
                    
                    String sql = sb.ToString();
                    connection.Open();
                    

                    // 수정 바람 #2 (4:32am)
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                await stepContext.Context.SendActivityAsync(MessageFactory.Text(reader.GetString(1)));
                                list.Add(reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동 이름 조회 /예외 발생"));

            }
            str = list.ToArray();
            // 수정 바람.#1
            foreach (var item in list)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동: " + (string)item ));

            }
            await stepContext.Context.SendActivityAsync(str.ToString());

            //{ "복근", "가슴", "팔", "다리", "어깨 등", "힙" })

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어떤 운동을 하셨나요??"),
                Choices = ChoiceFactory.ToChoices(new List<string> { str.ToString()})
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AskExerciseMinuteAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
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
            //결과 보여줘야 함

            return await stepContext.EndDialogAsync();

        }

        private static Task<bool> TimePromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            // This condition is our validation rule. You can also change the value at this point.
            return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0);
        }

    }
}
