using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace Test.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly UserState _userState; //수정?

        //실행중인 Dialog가 있는지 확인하는 용도. 
        public static int is_running_dialog = 0;


        public MainDialog(UserState userState) : base(nameof(MainDialog))
        {
            _userState = userState;

            AddDialog(new CheckUserDialog());       //사용자 확인(초기)
            AddDialog(new ShowFunctionsDialog());   //기능 카드 보여주기
            AddDialog(new TutorialDialog());        //튜토리얼
            AddDialog(new RecommendExerciseDialog());   //운동 추천
            AddDialog(new RecordDialog());          //운동 기록
            AddDialog(new RecommendFood());         //음식 추천
            AddDialog(new RecommendEquipment());    //기구 추천
            AddDialog(new SeeMyCharacterDialog());  //내 캐릭터 상태 보기
            AddDialog(new SeeMyRecord());           //내 기록 보기

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Dialog running 중인 상태를 1로 바꿉니다.
            is_running_dialog = 1;

            if (ModeManager.mode == (int)ModeManager.Modes.ShowFunction) //기능 보여주기
            {
                return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.Tutorial) //튜토리얼
            {
                return await stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.RecommendExercise) //운동 추천
            {
                return await stepContext.BeginDialogAsync(nameof(RecommendExerciseDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.Record) //운동 기록
            {
                return await stepContext.BeginDialogAsync(nameof(RecordDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.RecommendFood) //음식 추천
            {
                return await stepContext.BeginDialogAsync(nameof(RecommendFood), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.RecommendEquipment) //기구 추천
            {
                return await stepContext.BeginDialogAsync(nameof(RecommendEquipment), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.CheckCharacterState) //내 캐릭터 확인
            {
                return await stepContext.BeginDialogAsync(nameof(SeeMyCharacterDialog), null, cancellationToken);
            }
            else if (ModeManager.mode == (int)ModeManager.Modes.SeeMyRecord) //내 기록
            {
                return await stepContext.BeginDialogAsync(nameof(SeeMyRecord), null, cancellationToken);
            }
            else { //처음에 사용자 확인
                return await stepContext.BeginDialogAsync(nameof(CheckUserDialog), null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            is_running_dialog = 0; //다이얼로그 실행 중인 것이 없다.

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
