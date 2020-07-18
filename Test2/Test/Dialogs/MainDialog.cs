using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Test.Dialogs.Test.Dialogs;

namespace Test.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly UserState _userState;

        public MainDialog(UserState userState) : base(nameof(MainDialog))
        {
            _userState = userState;

            AddDialog(new TutorialDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var msg = "InitialStepAsync start";
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg, msg), cancellationToken);

            return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userInfo = (UserProfile)stepContext.Result;

            
            string status = "Thankyou "
                + (userInfo.UserName)
                + ".";

            await stepContext.Context.SendActivityAsync(status);

            var accessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            await accessor.SetAsync(stepContext.Context, userInfo, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
