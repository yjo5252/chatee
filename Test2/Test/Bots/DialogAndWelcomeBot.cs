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

            /*
            foreach (var member in membersAdded) {
                if (member.Id != turnContext.Activity.Recipient.Id) {
                    var reply = MessageFactory.Text($"Welcome to Complex Dialog Bot {member.Name}. " +
                        "This bot provides a complex conversation, with multiple dialogs. " +
                        "Type anything to get started.");
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }*/

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"안녕하세요! Healthee입니다."), cancellationToken);
                    await DisplayOptionsAsync(turnContext, cancellationToken);
                }
            }

        }

        private static async Task DisplayOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            // Create a HeroCard with options for the user to interact with the bot.
            var card = new HeroCard
            {
                Images = new List<CardImage> { new CardImage("https://w7.pngwing.com/pngs/271/1006/png-transparent-power-strength-gym-fitness-centre-physical-fitness-bodybuilding-physical-strength-workout-boxing-glove-logo-fictional-character.png") }

                // Images = new List<CardImage> { new CardImage("https://1.bp.blogspot.com/-055dCu7uAU0/XROZCKyjPPI/AAAAAAAAgzU/jLWMbfTdJXIHxxFC_0XrURGd0DUI5-1aQCEwYBhgL/s1600/2.jpg") }

            };

            var reply = MessageFactory.Attachment(card.ToAttachment());
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
