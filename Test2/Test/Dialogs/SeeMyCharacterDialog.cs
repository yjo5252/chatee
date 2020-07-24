using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

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
                EndAsync //끝내기
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ShowCharacterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("캐릭터를 확인해볼까요?"), cancellationToken);

            int state = UserInfoManager.ConversationCount; //현재는 기록을 한 횟수만큼 누적이 됨.

            if (UserInfoManager.ConversationCount > 5) state = 5;

            string[] ImageUrlList = {
                "https://img1.daumcdn.net/thumb/R800x0/?scode=mtistory2&fname=https%3A%2F%2Ft1.daumcdn.net%2Fcfile%2Ftistory%2F247A4D3D556FFF5224",//돼지[0]
                "https://pbs.twimg.com/media/ESjJsyqUUAEFnsD.jpg", //능이버섯[1]
                "https://t1.daumcdn.net/section/oc/b91f421cab1a46dd96a845f0c55b7f91", //가취가욥[2]
                "https://1.gall-img.com/hygall/files/attach/images/82/921/061/279/481b3fedcc3b8dcabdff8c9c6a0bfcda.jpg",//농담곰 천재[3]
                "https://lh3.googleusercontent.com/proxy/FLiXOTZpuXPEEYDMoOaFozAlSkMXpSR4XKMN7ffTtAGXUHXhZeLxUKPsM_h4bAvRscoQS_HhRhQhfGH6kjtx2yI_Mdr_8ODLX8WEud8LRMqmJ6lKurfqh7J-ivyycZw6tkTZXrFkpHWJDbqor-muWAaVTgL9_RBexYUVy0QNGovSeJka3MeF0P34cA",//시바견[4]
                "https://image.librewiki.net/thumb/1/10/%EA%B7%BC%EC%9C%A1%EB%AA%AC.png/360px-%EA%B7%BC%EC%9C%A1%EB%AA%AC.png"//근육몬 [5]
            };

            string[] command = {
            "아직 운동을 안하셨군요...운동을 하고 기록해주세요!",//[0]
            "지금은 능이버섯이지만 운동을 조금만 더 열심히 하면 더 건강해지실거에요.",//[1]
            "운동을 하셔서 활기가 넘치시네요!",//[2]
            "운동 천재입니다. ^^",//[3]
            "멀리서 봐도 근육이 보이는 경지에 이르셨어요.",//[4]
            "근육 그 자체시네요.",//[5]
            };

            //캐릭터 상태 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);


            var heroCard = new HeroCard
            {
                Title = "#### " + UserInfoManager.UserName + "님의 캐릭터의 상태입니다.",
                Images = new List<CardImage> { new CardImage(ImageUrlList[state]) },
                Text = command[state]
            };

            reply.Attachments.Add(heroCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            Thread.Sleep(3000);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            //끝내기
            return await stepContext.EndDialogAsync();
        }



    }
}
