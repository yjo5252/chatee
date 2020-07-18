using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Dialogs
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Choices;
    using Microsoft.Bot.Schema;

    namespace Test.Dialogs
    {
        public class TutorialDialog : ComponentDialog
        {

            // Define a "done" response for the company selection prompt.
            private const string DoneOption = "done";

            // Define value names for values tracked inside the dialogs.
            private const string UserInfo = "value-userInfo";

            public TutorialDialog() : base(nameof(TutorialDialog))
            {
                AddDialog(new TextPrompt(nameof(TextPrompt)));
                AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
                AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), WeightPromptValidatorAsync));
                AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

                AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                NameStepAsync,
                TargetStepAsync,
                KnowHowStepAsync,
                PreWeightStepAsync,
                PostWeightStepAsync,
                DateStepAsync,
                ShowAvatarStepAsync,
                AlarmStepAsync,
                AcknowledgementStepAsync,
            }));

                InitialDialogId = nameof(WaterfallDialog);
            }

            private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                // Create an object in which to collect the user's information within the dialog.
                stepContext.Values[UserInfo] = new UserProfile();

                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"안녕하세요! \n\n 여러분과 함께 운동할 Healthee입니다!"), cancellationToken);

                var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("이름을 알려주세요!") };

                // Ask the user to enter their name.
                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
            }

            private async Task<DialogTurnResult> TargetStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.UserName = (string)stepContext.Result;

                var promptOptions = new PromptOptions {
                    Prompt = MessageFactory.Text($"{stepContext.Result}님의 운동의 목표가 뭘까요? \n\n 다이어트, 근력, 유연성 중 하나를 골라주세요!"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "다이어트", "근력 운동", "유연성" })
                };

                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            }

            private async Task<DialogTurnResult> KnowHowStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.Target = ((FoundChoice)stepContext.Result).Value;

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("운동을 많이 해보셨나요? 상 중 하 중 하나를 골라주세요! "),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "상", "중", "하" })
                };

                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"이전단계, {((UserProfile)stepContext.Values[UserInfo]).Target}."),
                    cancellationToken);

                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            }


            private async Task<DialogTurnResult> PreWeightStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.KnowHow = ((FoundChoice)stepContext.Result).Value;

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("현재 체중을 알려주세요"),
                    RetryPrompt = MessageFactory.Text("0보다 크고 300보다 작은 수치로 적어주세요."),
                };

                return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);

            }

            private async Task<DialogTurnResult> PostWeightStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.PreWeight = (int)stepContext.Result;

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("목표 체중을 알려주세요"),
                    RetryPrompt = MessageFactory.Text("0보다 크고 300보다 작은 수치로 적어주세요."),
                };

                return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
            }

            private async Task<DialogTurnResult> DateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.PostWeight = (int)stepContext.Result;

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("목표 기간을 알려주세요"),
                    RetryPrompt = MessageFactory.Text("날짜형식으로 작성해주세요."), // 입력받은 날짜는 현재로부터 D-day 기능
                };

                return await stepContext.PromptAsync(nameof(DateTimePrompt), promptOptions, cancellationToken); // DateTimePrompt 형식 설정?

            }

            private async Task<DialogTurnResult> ShowAvatarStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.Date = stepContext.Result.ToString();

                var card = new HeroCard
                {
                    Images = new List<CardImage> { new CardImage("https://img1.daumcdn.net/thumb/R580x0/?fname=http%3A%2F%2Fimg1.daumcdn.net%2Falexander%2Fattach%2F2yb6eib1j9ekg43plpow7y14iiz7u9vff") }

                };

                var reply = MessageFactory.Attachment(card.ToAttachment());
                await stepContext.Context.SendActivityAsync(reply, cancellationToken);

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("캐릭터의 이름을 설정해주세요!"),
                };

                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

            }

            private async Task<DialogTurnResult> AlarmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {

                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.AvatarName = (string)stepContext.Result;


                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text($"몇 시에  {(string)stepContext.Result}과 같이 운동하실건가요?")
                };

                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
            }


            private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                // Set the user's company selection to what they entered in the review-selection dialog.
                var userProfile = (UserProfile)stepContext.Values[UserInfo];
                userProfile.AlarmSetup = (string)stepContext.Result;
                /*
                userProfile.CompaniesToReview = stepContext.Result as List<string> ?? new List<string>();

                */

                var msg = $"{userProfile.UserName}님 {userProfile.Date}일 기간동안 Healthee와 {userProfile.Target} 를 열심히 운동해보아요!\n "
                            + $"{ userProfile.PreWeight}에서 { userProfile.PostWeight}으로 체중감량이 이루어질거에요!\n"
                            + $"{ userProfile.UserName}님의 캐릭터 { userProfile.AvatarName} 변화도 눈여겨봐주세요!";

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

                // Thank them for participating.
                await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Thanks for participating, {((UserProfile)stepContext.Values[UserInfo]).UserName}."),
                        cancellationToken);

                    // Exit the dialog, returning the collected user information.
                    return await stepContext.EndDialogAsync(stepContext.Values[UserInfo], cancellationToken);
                
            }

            private static Task<bool> WeightPromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
            {
                // This condition is our validation rule. You can also change the value at this point.
                return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0 && promptContext.Recognized.Value < 300);
            }


        }
    }

}
