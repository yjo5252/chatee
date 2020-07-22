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
    public class SeeMyCharacterDialog : ComponentDialog
    {
        public SeeMyCharacterDialog() : base(nameof(SeeMyCharacterDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                ShowCharacterAsync, //캐릭터 보여주기
                EndAsync //추천
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ShowCharacterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            //캐릭터 상태 보여주기

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(""),
            };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            //끝내기
            return await stepContext.EndDialogAsync();
        }


    }
}
