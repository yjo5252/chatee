// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Net.Http;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AdaptiveCards;
using System.IO;
using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Web;

namespace EchoBot.Bots
{
    public class QnABot : ActivityHandler

    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<QnABot> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {

            await base.OnTurnAsync(turnContext, cancellationToken);

            if (turnContext.Activity.Value != null)
            {
                var txt = turnContext.Activity.Text;
                dynamic val = turnContext.Activity.Value;
                await turnContext.SendActivityAsync(MessageFactory.Text("1","1"), cancellationToken);

                // Check if the activity came from a submit action
                if (string.IsNullOrEmpty(txt) && val != null)
                {
                    // Retrieve the data from the id_number field
                    //var num = val.id_number;
                    
                    //await turnContext.SendActivityAsync(MessageFactory.Text((string)num, (string)num), cancellationToken);
                }
                
                await turnContext.SendActivityAsync(turnContext.Activity.Value.ToString());
            }

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var response = turnContext.Activity.CreateReply();
                response.Attachments = new List<Attachment>() { CreateAdaptiveCardUsingJson() };

                await turnContext.SendActivityAsync(response);
            }


            }

            private Attachment CreateAdaptiveCardUsingJson()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText("Cards/Test.json")).Card
            };
 
        }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
