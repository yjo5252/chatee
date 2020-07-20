using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema; 
using Microsoft.Extensions.Logging;

namespace Test.Dialogs
{
    public class ShowFunctionsDialog : ComponentDialog
    {

        public ShowFunctionsDialog()
            : base(nameof(ShowFunctionsDialog))
        {
           // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            string status = "ok";

            await stepContext.Context.SendActivityAsync(status);

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();

            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            // Display an Adaptive Card
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var heroCard = new HeroCard
            {
                Title = "#### 1. 운동 추천",
                Text = "운동 부위, 기구 유무, 운동 종류를 설정하여 맞춤 운동을 추천받아보세요! 운동 방법과 운동할 세트, 시간, 자세 등 적절한 운동과 정보를 알려드립니다.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 추천받기", value: "운동 추천 받을래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 2. 운동 기록",
                Text = "운동한 것을 기록해보세요! 운동 이름, 시간, 운동한 부위를 기록할 수 있습니다. 운동을 많이 하고 기록할 수록 캐릭터가 성장해요!",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 기록하기", value: "운동 기록할래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 3. 음식 추천",
                Text = "운동하고 싶은 부위와 운동의 종류를 골라 맞춤 음식을 추천받으세요!",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "음식 추천받기", value: "음식 추천해줘") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 4. 운동 기구 추천",
                Text = "운동하고 싶은 부위와 운동 종류에 따라 맞춤 운동 기구를 추천받으세요! 운동 기구를 살 수 있는 곳도 알려드립니다.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 기구 추천받기", value: "운동 기구 추천해줘") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 5. 알림",
                Text = "운동할 시간을 설정하여 정해진 시간에 알림을 받으세요.w",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "알림 설정하기", value: "알림 설정할래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another card."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }
    }
}
