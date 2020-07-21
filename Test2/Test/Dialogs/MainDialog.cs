using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using Test.Dialogs.Test.Dialogs;

namespace Test.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly UserState _userState;
        public static int tutorial = 0;
        public static int mode = 0;
        public static int is_running_dialog = 0;


        public MainDialog(UserState userState) : base(nameof(MainDialog))
        {
            _userState = userState;

            AddDialog(new TutorialDialog());
            AddDialog(new ShowFunctionsDialog());
            AddDialog(new RecommendExerciseDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            is_running_dialog = 1;

            var msg = $"MainDialog.cs InitialStep Async";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(is_running_dialog.ToString()), cancellationToken);

            if (mode == 0)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 0입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);

            }
            else {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 0이 아닙니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(RecommendExerciseDialog), null, cancellationToken);
            }
            /*
            else if (mode == 1) { 
            }
            else if (mode == 1)
            {
            }
            else if (mode == 1)
            {
            }
            else if (mode == 1)
            {
            }
            else if (mode == 1)
            {
            }*/
            //return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userInfo = (UserProfile)stepContext.Result;

            /*
            string status = "튜토리얼이 끝났습니다 "
                + (userInfo.UserName)
                + "님!";

            await stepContext.Context.SendActivityAsync(status);

            tutorial = 1;

            var accessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            await accessor.SetAsync(stepContext.Context, userInfo, cancellationToken);
            */
            is_running_dialog = 0;
            var msg = $"MainDialog.cs finalStepAsync";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
