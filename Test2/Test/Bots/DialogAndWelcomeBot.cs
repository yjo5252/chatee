using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Test.Dialogs;

namespace Test.Bots
{
    public class DialogAndWelcomeBot<T> : EchoBot<T> where T : Dialog
    {

        public DialogAndWelcomeBot(IConfiguration configuration, IHttpClientFactory httpClientFactory, ConversationState conversationState, UserState userState, T dialog, ILogger<EchoBot<T>> logger) : base(configuration, httpClientFactory, conversationState, userState, dialog, logger) { }

        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"안녕하세요! Healthee입니다."), cancellationToken);
                    MainDialog.tutorial = 0;
                    await DisplayOptionsAsync(turnContext, cancellationToken);
                }
            }

        }

        private static async Task DisplayOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            // Create a HeroCard with options for the user to interact with the bot.
            var card = new HeroCard
            {
                Images = new List<CardImage> { new CardImage("https://user-images.githubusercontent.com/41981471/87846811-b6fccc80-c90d-11ea-9a1c-e136034394f0.png") }
            };

            var reply = MessageFactory.Attachment(card.ToAttachment());
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
