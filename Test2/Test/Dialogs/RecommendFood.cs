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
    public class RecommendFood : ComponentDialog
    {
        public RecommendFood() : base(nameof(RecommendFood))
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
                Prompt = MessageFactory.Text($"어떤 부위에 좋은 음식을 추천해드릴까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "가슴", "팔", "다리", "어깨 등", "힙" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RecommendResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            //결과 보여줘야 함

            return await stepContext.EndDialogAsync();
        }
    }
}
