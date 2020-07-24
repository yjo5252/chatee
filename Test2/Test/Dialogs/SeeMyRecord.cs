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
    public class SeeMyRecord : ComponentDialog
    {

        public SeeMyRecord() : base(nameof(SeeMyRecord))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                ShowRecordAsync //기록 보여주기
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> ShowRecordAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            //운동 기록 보여주기

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
            return await stepContext.EndDialogAsync();
        }
    }
}
