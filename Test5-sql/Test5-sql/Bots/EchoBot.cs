// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Test5_sql.Bots
{
    public class EchoBot : ActivityHandler

    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    await turnContext.SendActivityAsync(MessageFactory.Text("connected"), cancellationToken);

                    string query = "INSERT INTO [dbo].[Persons] VALUES ( 'lalala', 'quququ', 30);";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    /*
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                await turnContext.SendActivityAsync(MessageFactory.Text("ok"), cancellationToken);
                                Console.WriteLine(reader.GetString(0));
                                await turnContext.SendActivityAsync(MessageFactory.Text(reader.GetString(0)), cancellationToken);
                            }
                        }

                    }*/
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            await turnContext.SendActivityAsync(MessageFactory.Text("hi"), cancellationToken);
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
