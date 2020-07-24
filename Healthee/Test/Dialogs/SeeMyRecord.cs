using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Test.Dialogs
{
    public class SeeMyRecord : ComponentDialog
    {
        private int Abs;
        private int Leg;
        private int Arm;
        private int Shoulder;
        private int Chest;

        public SeeMyRecord() : base(nameof(SeeMyRecord))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[] {
                ShowRecordAsync, //기록 보여주기
                EndAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> ShowRecordAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("지금까지 운동한 기록을 볼까요?"), cancellationToken);

            Thread.Sleep(1500);

            //변수 초기화
            Abs = 0;
            Leg = 0;
            Arm = 0;
            Shoulder = 0;
            Chest = 0;

            int sum = 0; //전체 운동 시간

            //운동 기록 보여주기
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString = "Server=tcp:team02-server.database.windows.net,1433;Initial Catalog=healtheeDB;Persist Security Info=False;User ID=chatbot02;Password=chatee17!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    //기록 DB에 사용자가 있는지 찾는다.
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기
                    String sql = sb.ToString();

                    int count = 0;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                count = reader.GetInt32(0);
                            }
                        }
                    }

                    SqlCommand q;
                    string query;

                    if (count == 0) //기록이 없을 때
                    {
                        
                        query = "INSERT INTO [dbo].[ExerciseRecord] VALUES(" + UserInfoManager.keyNum + ", 0, 0, 0, 0, 0);"; //사용자 기록을 만든다.
                        q = new SqlCommand(query, connection);
                        q.ExecuteNonQuery();
                    }

                    //기록 읽어옴
                    sb = new StringBuilder();
                    sb.Append("SELECT * FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기
                    sql = sb.ToString();


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Abs = reader.GetInt32(1);
                                Leg = reader.GetInt32(2);
                                Arm = reader.GetInt32(3);
                                Shoulder = reader.GetInt32(4);
                                Chest = reader.GetInt32(5);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"운동 /예외 발생"));

            }

            sum = Abs + Leg + Arm + Shoulder + Chest;

            var receiptCard = new ReceiptCard
            {
                Title = "운동 기록",
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem(
                        "Abs",
                        price: (int)(Abs/60)+"분",
                        image: new CardImage(url: "https://static.independent.co.uk/s3fs-public/thumbnails/image/2018/03/29/15/istock-157741609.jpg?w968h681")),
                    new ReceiptItem(
                        "Leg",
                        price: (int)(Leg/60)+"분",
                        image: new CardImage(url: "https://www.muscleandfitness.com/wp-content/uploads/2013/08/muscular-legs.jpg")),
                     new ReceiptItem(
                        "Arm",
                        price: (int)(Arm/60)+"분",
                        image: new CardImage(url: "https://i0.wp.com/ascentchiropractic.com/wp-content/uploads/2018/04/shutterstock_162592241.jpg?fit=1000%2C662&ssl=1")),
                      new ReceiptItem(
                        "Shoulder",
                        price: (int)(Shoulder/60)+"분",
                        image: new CardImage(url: "https://manofmany.com/wp-content/uploads/2019/03/10-Best-Shoulder-Exercises-for-Men-Man-lifting-weights-shoulder-muscle-1280x720.jpg")),
                       new ReceiptItem(
                        title: "Chest",
                        price: (int)(Chest/60)+"분",
                        image: new CardImage(url: "https://fitnessdreaming.com/wp-content/uploads/2017/02/chest_muscle.jpg")),
                }
            };

            var reply = MessageFactory.Attachment(receiptCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("총 "+(int)(sum/60)+"분을 운동하셨네요!"), cancellationToken);

            ModeManager.mode = (int)ModeManager.Modes.ShowFunction; //기능 보기 모드로 바꾼다.
            Thread.Sleep(3000);
            return await stepContext.BeginDialogAsync(nameof(ShowFunctionsDialog), null, cancellationToken);
            //return await stepContext.EndDialogAsync();
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { //-1
            //끝내기
            return await stepContext.EndDialogAsync();
        }
    }
}
