using System;
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
          

            string status = "ok";
            await stepContext.Context.SendActivityAsync(status);

            var attachments = new List<Attachment>();

            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            string[] ImageUrlList = {"https://t1.daumcdn.net/section/oc/b91f421cab1a46dd96a845f0c55b7f91",
                "https://image.librewiki.net/thumb/1/10/%EA%B7%BC%EC%9C%A1%EB%AA%AC.png/360px-%EA%B7%BC%EC%9C%A1%EB%AA%AC.png"};
            int index =1 ;
            var heroCard = new HeroCard
            {
                Title = "#### 캐릭터의 상태"+ index,
                Images = new List<CardImage> { new CardImage(ImageUrlList[index]) }
            };
            reply.Attachments.Add(heroCard.ToAttachment());


            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.

            //끝내기
            return await stepContext.EndDialogAsync();
        }


    }
}
