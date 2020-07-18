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
        public static int tutorial = 0;

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
            return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userInfo = (UserProfile)stepContext.Result;

            
            string status = "튜토리얼이 끝났습니다 "
                + (userInfo.UserName)
                + "님!";

            await stepContext.Context.SendActivityAsync(status);

            tutorial = 1;

            var accessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            await accessor.SetAsync(stepContext.Context, userInfo, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
