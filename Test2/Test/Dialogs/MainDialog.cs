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
        public static int is_running_dialog = 0;


        public MainDialog(UserState userState) : base(nameof(MainDialog))
        {
            _userState = userState;

            AddDialog(new ShowFunctionsDialog());
            AddDialog(new TutorialDialog());
            AddDialog(new RecommendExerciseDialog());
            AddDialog(new RecordDialog());
            AddDialog(new RecommendFood());
            AddDialog(new RecommendEquipment());
            AddDialog(new SeeMyCharacterDialog());
            AddDialog(new SeeMyRecord());

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

            if (ModeManager.mode == 0)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 0입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == 1)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 1입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);

            }
            else if (ModeManager.mode == 2)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 2입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(RecommendExerciseDialog), null, cancellationToken);

            }
            else if (ModeManager.mode == 3) {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 3입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(RecordDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == 4) {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 4입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(RecommendFood), null, cancellationToken);
            }
            else if (ModeManager.mode == 5) {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 5입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(RecommendEquipment), null, cancellationToken);
            }
            else if (ModeManager.mode == 6) {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 6입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(SeeMyCharacterDialog), null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("mode가 현재 7입니다."), cancellationToken);
                return await stepContext.BeginDialogAsync(nameof(SeeMyRecord), null, cancellationToken);
            }
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
