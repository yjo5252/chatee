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
            foreach (var member in membersAdded) {
                if (member.Id != turnContext.Activity.Recipient.Id) {
                    var reply = MessageFactory.Text($"Welcome to Complex Dialog Bot {member.Name}. " +
                        "This bot provides a complex conversation, with multiple dialogs. " +
                        "Type anything to get started.");
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }
        
        }
    }
}
