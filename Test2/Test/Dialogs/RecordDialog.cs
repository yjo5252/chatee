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
                AskUserWhatExerciseAsync,
                CheckExerciseAsync,
                AskExerciseMinuteAsync,
                SelectAreaAsync,
                SummaryAsync

            }));
            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> AskUserWhatExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions { 
                Prompt = MessageFactory.Text("오늘은 어떤 운동을 하셨나요?") ,
                RetryPrompt = MessageFactory.Text("응이라고 입력해주세요.")
            };

            // Ask the user to enter their name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckExerciseAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("체크할 부분입니다."), cancellationToken);

            return await stepContext.EndDialogAsync();
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

        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"어디를 운동하셨나요??"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근","가슴", "팔", "다리" ,"어깨 등","힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
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
