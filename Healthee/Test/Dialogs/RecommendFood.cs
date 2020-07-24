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
        private string foodName;
        private string ImageUrl;
        private string Outline;


        public RecommendFood() : base(nameof(RecommendFood))
        {

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                InitialAsync, //카드 보여주는 부분
                RecommendResultAsync, // 음식정보 카드 
                EndAsync //끝내기
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //변수 초기화
            foodName = "";
            ImageUrl = "";
            Outline = "";

            //처음 카드 보여주기
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("FoodInitial.json"));
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            Thread.Sleep(1500);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("랜덤으로 음식을 추천해드려요!"), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("추천해 드리는 음식은..."), cancellationToken);

            Thread.Sleep(1500);

            return await stepContext.NextAsync();

        }
            private async Task<DialogTurnResult> RecommendResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Random r = new Random();
                    int randomnumber = r.Next(1, 28);

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [dbo].[Food] WHERE Number=" + randomnumber);
                    String sql = sb.ToString();


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                foodName = (string)reader.GetValue(1);
                                ImageUrl = (string)reader.GetValue(2);
                                Outline = (string)reader.GetValue(3);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"예외 발생"));

            }

            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);

            var heroCard = new HeroCard
            {
                Images = new List<CardImage> { new CardImage(ImageUrl) },
                Title = "#### 오늘의 추천 음식: " + foodName,
                Text = Outline,

            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction;
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);

        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
