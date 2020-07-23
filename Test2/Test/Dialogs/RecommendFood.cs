using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;

using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Http;

namespace Test.Dialogs
{
    public class RecommendFood : ComponentDialog
    {
        public string foodName = "고구마";
        public string ImageUrl = "url";
        public string Outline = "outline";


        public RecommendFood() : base(nameof(RecommendFood))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                SelectAreaAsync, //어떤 부위
                RecommendResultAsync, //추천
                ShowFoodCardAsync // 음식정보 카드 
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

       

        private async Task<DialogTurnResult> SelectAreaAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"음식 추천을 시작할까요?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "시작하기" })
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RecommendResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text($"추천을 시작합니다.") }; ;
            String[] str = { };
            List<String> list = new List<String>();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * ");
                    sb.Append("FROM [dbo].[Food] pc ");

                   
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                list.Add(reader.GetString(0));
                                //await turnContext.SendActivityAsync(reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                  /*
                    foreach (var item in list)
                    {
                        promptOptions = new PromptOptions { Prompt = MessageFactory.Text(item) };
                        return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
                    }
                 */
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"예외 발생"));
               
            }
            
            Random food = new Random();
            int randomIndex = 0;
            str = list.ToArray();
          //  String [] str = { "고구마", "고기 구이", "블루베리" };
            randomIndex = food.Next(str.Length);
            foodName = (string)str[randomIndex];

           await stepContext.Context.SendActivityAsync(MessageFactory.Text($"오늘의 추천음식은 " + (string)str[randomIndex]+" 입니다."));
          
           return await stepContext.NextAsync();
        }


        private async Task<DialogTurnResult> ShowFoodCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
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


            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * ");
                    sb.Append("FROM [dbo].[Food] pc ");
                    sb.Append("WHERE [Name]='"+ foodName+"'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                ImageUrl = reader.GetString(2);
                                Outline = reader.GetString(3);
                               // await stepContext.Context.SendActivityAsync(MessageFactory.Text(reader.GetString(1)));

                               // await stepContext.Context.SendActivityAsync(MessageFactory.Text(foodName));

                            }
                        }
                    }
                    connection.Close();

                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"음식카드 /예외 발생"));

            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(ImageUrl));
            var heroCard = new HeroCard
            {
                Title = "#### 오늘의 추천 음식: "+ foodName,
                Text = Outline,
                Images = new List<CardImage> { new CardImage(ImageUrl) },
                
            };
            reply.Attachments.Add(heroCard.ToAttachment());


            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            return await stepContext.EndDialogAsync();
        }



    }
}
