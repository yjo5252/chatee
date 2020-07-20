using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Test.Dialogs
{
    public class RecommendExerciseDialog : ComponentDialog
    {

        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        public RecommendExerciseDialog() : base(nameof(RecommendExerciseDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                RecommendInitialAsync,
                CheckAnswerAsync,
                SelectAreaAsync,
                ShowExerciseAsync,

            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult>RecommendInitialAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.UserName = (string)stepContext.Result;

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"{userProfile.UserName}님의 맞춤 운동을 추천해드릴까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "응", "아니" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckAnswerAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string check = (string)stepContext.Result;

            if (check.Equals("응"))
            {
                return await stepContext.NextAsync();
            }
            else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 1;
                return await stepContext.NextAsync();
            }
        }

        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"운동하고 싶은 부위를 선택해주세요!"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "가슴", "팔", "다리", "어꺠 및 등", "힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"운동하고 싶은 부위를 선택해주세요!"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "가슴", "팔", "다리", "어꺠 및 등", "힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

    }
}
