using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Text;

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
            NameStepAsync, //사용자 이름
            AreaStepAsync, //부위
            TargetStepAsync, //운동 종류
            KnowHowStepAsync, //숙련도
            PreWeightStepAsync, //현재 체중
            PostWeightStepAsync, //목표 체중
            ShowAvatarStepAsync, //캐릭터 보여주기
            AvatarNameAsync, //캐릭터 이름 설정
            AcknowledgementStepAsync, //결과 보여주기
        }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Create an object in which to collect the user's information within the dialog.
            stepContext.Values[UserInfo] = new UserProfile();

            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("StartTutorial.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"안녕하세요! \n\n 여러분과 함께 운동할 Healthee입니다! \n\n 튜토리얼을 진행하도록 하겠습니다."), cancellationToken);

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("이름을 알려주세요!") };

            // Ask the user to enter their name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AreaStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.UserName = (string)stepContext.Result.ToString().Trim(); //유저 이름에 할당

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"{stepContext.Result}님은 어디를 제일 운동하고 싶으신가요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "복근", "다리, 힙", "팔", "어깨 및 등", "가슴" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }


        private async Task<DialogTurnResult> TargetStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Area = ((FoundChoice)stepContext.Result).Value; //목표 부위에 할당

            var promptOptions = new PromptOptions {
                Prompt = MessageFactory.Text($"{userProfile.UserName}님의 운동의 목표가 뭘까요? \n\n 유산소, 근력, 스트레칭 중 하나를 골라주세요!"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "유산소", "근력", "스트레칭" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> KnowHowStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Category = ((FoundChoice)stepContext.Result).Value; //운동 종류에 할당

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("운동을 많이 해보셨나요? 운동 실력을 상 중 하 중 하나를 골라주세요! "),
                Choices = ChoiceFactory.ToChoices(new List<string> { "상", "중", "하" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }


        private async Task<DialogTurnResult> PreWeightStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.SkillLevel = ((FoundChoice)stepContext.Result).Value; //숙련도에 할당

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("현재 체중을 알려주세요(단위 : kg)"),
                RetryPrompt = MessageFactory.Text("0보다 크고 300보다 작은 수치로 적어주세요."),
            };
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);

        }

        private async Task<DialogTurnResult> PostWeightStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.PreWeight = (int)stepContext.Result; //이전 체중에 할당

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("목표 체중을 알려주세요(단위 : kg)"),
                RetryPrompt = MessageFactory.Text("0보다 크고 300보다 작은 수치로 적어주세요."),
            };

            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowAvatarStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
        var userProfile = (UserProfile)stepContext.Values[UserInfo];
        userProfile.PostWeight = (int)stepContext.Result; //목표 체중에 할당

        var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("CharacterShow.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(""),
            };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

        }

        private async Task<DialogTurnResult> AvatarNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("CharacterName.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(""),
            };


            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the user's company selection to what they entered in the review-selection dialog.
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            /*
            userProfile.CompaniesToReview = stepContext.Result as List<string> ?? new List<string>();
            */
            var msg = $"{userProfile.UserName}님!\nHealthee와 {userProfile.Area}를 위해 {userProfile.Category} 운동을 열심히 해봐요!\n"
                        + $"{ userProfile.PreWeight}kg에서 { userProfile.PostWeight}kg으로 체중 감량이 이루어질거에요!\n"
                        + $"{ userProfile.UserName}님의 캐릭터 { userProfile.AvatarName} 변화도 눈여겨봐주세요!";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            UserInfoManager.UserName = userProfile.UserName;
            UserInfoManager.PreWeight = userProfile.PreWeight;
            UserInfoManager.PostWeight = userProfile.PostWeight;
            UserInfoManager.SkillLevel = userProfile.SkillLevel;
            UserInfoManager.Area = userProfile.Area;
            UserInfoManager.Category = userProfile.Category;
            UserInfoManager.ConversationCount = 0;
            UserInfoManager.AvatarName = userProfile.AvatarName;
            UserInfoManager.AvatarState = userProfile.AvatarState;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    string query1 = "INSERT INTO [dbo].[UserInfo] VALUES( " + CheckUserDialog.PrimaryNumber + ", '" + userProfile.UserName + "', " + userProfile.PreWeight + ", " + userProfile.PostWeight + ", '" + userProfile.SkillLevel + "', '" + userProfile.Area + "', '" + userProfile.Category + "', " + 0 + ");";
                    string query2 = "INSERT INTO [dbo].[AvatarInfo] VALUES( " + CheckUserDialog.PrimaryNumber + ", '" + userProfile.AvatarName + "', " + 0 + ");";

                    SqlCommand command = new SqlCommand(query1, connection);
                    command.ExecuteNonQuery();

                    command = new SqlCommand(query2, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("문제가 있습니다."), cancellationToken);
                Console.WriteLine(e.ToString());
            }

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.

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


