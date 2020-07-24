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
                ShowResultStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
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
                Text = "운동 부위, 운동 종류를 설정하여 맞춤 운동을 추천받아보세요! 운동 방법과 운동할 세트, 시간, 자세 등 운동 방법을 영상과 함께 알려드립니다.",
                Images = new List<CardImage> { new CardImage("https://static01.nyt.com/images/2016/08/07/health/7-minute-workout-cover/7-minute-workout-cover-largeHorizontal375.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 추천받기", value: "운동 추천 받을래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 2. 운동 기록",
                Text = "운동한 것을 기록해보세요! 운동한 시간, 운동한 부위를 기록할 수 있습니다. 운동을 많이 하고 기록할 수록 캐릭터가 성장해요!",
                Images = new List<CardImage> { new CardImage("https://static01.nyt.com/images/2016/08/10/health/10-minute-workout-cover/10-minute-workout-cover-jumbo.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 기록하기", value: "운동 기록할래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 3. 음식 추천",
                Text = "랜덤으로 음식을 추천받으세요!",
                Images = new List<CardImage> { new CardImage("https://images.assetsdelivery.com/compings_v2/logo3in1/logo3in11811/logo3in1181100007.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "음식 추천받기", value: "음식 추천해줘") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 4. 운동 기구 추천",
                Text = "운동하고 싶은 부위에 따라 맞춤 운동 기구를 추천받으세요! 운동 기구를 살 수 있는 곳과 이용 방법 영상도 함께 알려드립니다.",
                Images = new List<CardImage> { new CardImage("https://static.vecteezy.com/system/resources/previews/000/261/475/non_2x/set-of-different-gym-or-fitness-equipment-and-training-apparatus-vector.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 기구 추천받기", value: "운동 기구 추천해줘") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 5. 내 캐릭터 보기",
                Text = "현재 캐릭터의 상태를 확인하세요! 캐릭터는 운동 기록을 많이 할수록 성장합니다.",
                Images = new List<CardImage> { new CardImage("https://i.pinimg.com/originals/8b/56/1c/8b561cd99fcab050edcbaf244c5ee4b6.png") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "내 캐릭터 보기", value: "캐릭터 볼래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());

            heroCard = new HeroCard
            {
                Title = "#### 6. 내 운동 기록 보기",
                Text = "지금까지 운동한 기록을 화인하세요!",
                Images = new List<CardImage> { new CardImage("https://cdn.dribbble.com/users/2309351/screenshots/6831637/workout.png") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "운동 기록 보기", value: "기록 볼래") },
            };
            reply.Attachments.Add(heroCard.ToAttachment());


            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next

            return await stepContext.NextAsync();

        }

        private async Task<DialogTurnResult> ShowResultStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var msg = (string)stepContext.Result;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
            return await stepContext.EndDialogAsync();

        }
    }
}
