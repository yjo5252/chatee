// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Bot.Builder.Dialogs;
using Test.Dialogs;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using AdaptiveCards;
using System.IO;

namespace Test.Bots
{
    public class EchoBot<T> : ActivityHandler where T : Dialog
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;
        protected readonly ILogger Logger;

        public EchoBot(IConfiguration configuration, IHttpClientFactory httpClientFactory, ConversationState conversationState, UserState userState, T dialog, ILogger<EchoBot<T>> logger) {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            Logger = logger;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default) {

            await base.OnTurnAsync(turnContext, cancellationToken);

            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);

        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //QnAMaker 불러오기
            var httpClient = _httpClientFactory.CreateClient();
            var qnaMaker = new QnAMaker(new QnAMakerEndpoint
                {
                    KnowledgeBaseId = _configuration["QnAKnowledgebaseId"],
                    EndpointKey = _configuration["QnAEndpointKey"],
                    Host = _configuration["QnAEndpointHostName"]
                },
            null,
            httpClient);
            Logger.LogInformation("Running dialog with Message Activity.");

            //사용자 입력은?
            string msg_from_user = turnContext.Activity.Text;
            msg_from_user = msg_from_user.Replace(" ","");

            if (MainDialog.is_running_dialog == 0)
            {

                if (msg_from_user.Contains("운동추천"))
                {
                    MainDialog.mode = 1;
                    await turnContext.SendActivityAsync(MessageFactory.Text("1"), cancellationToken);
                }
                else if (msg_from_user.Contains("운동기록"))
                {
                    MainDialog.mode = 2;
                    await turnContext.SendActivityAsync(MessageFactory.Text("2"), cancellationToken);
                }
                else if (msg_from_user.Contains("음식추천"))
                {
                    MainDialog.mode = 3;
                    await turnContext.SendActivityAsync(MessageFactory.Text("3"), cancellationToken);
                }
                else if (msg_from_user.Contains("운동기구추천"))
                {
                    MainDialog.mode = 4;
                    await turnContext.SendActivityAsync(MessageFactory.Text("4"), cancellationToken);
                }
                else if (msg_from_user.Contains("알림설정"))
                {
                    MainDialog.mode = 5;
                    await turnContext.SendActivityAsync(MessageFactory.Text("5"), cancellationToken);
                }
                else
                {
                    MainDialog.mode = 0;
                    //await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
                    //await turnContext.SendActivityAsync(MessageFactory.Text(turnContext.Activity.Text), cancellationToken);
                }
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);

            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("is_running_dialog가 1입니다."), cancellationToken);
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
            }
            //await turnContext.SendActivityAsync(MessageFactory.Text(MainDialog.is_running_dialog.ToString()), cancellationToken);

            /*
            if (MainDialog.tutorial == 0)
                //공통
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
            else {
                var options = new QnAMakerOptions { Top = 1 };

                // The actual call to the QnA Maker service.
                var response = await qnaMaker.GetAnswersAsync(turnContext, options);


                if (response != null && response.Length > 0)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(response[0].Answer), cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
                }

            }*/

        }

    }
}
