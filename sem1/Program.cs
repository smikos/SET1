using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace sem1
{
    class Program
    {
        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private CancellationToken ct = cts.Token;
         static string  mems;
        static void Server()
        {
            string name = "Server";
            
            while (!ct.IsCancellationRequested)
            { 
            // Инициализация объектов UdpClient и IPEndPoint
            UdpClient udpClient;
            IPEndPoint remoteEndPoint;
            // Создание экземпляра UdpClient, привязка к порту 12345 и указание любого адреса
            udpClient = new UdpClient(12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("UDP Клиент ожидает сообщений...");
                while (true)
                {
                    byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    try
                    {

                        var message = Message.FromJson(receivedData); // Десериализация сообщения из JSON


                        if (message.Text != "1234")
                        {
                            Console.WriteLine($"Получено сообщение от {message.FromName} ({message.Date}):");
                            Console.WriteLine(message.Text);
                            Console.Write("Введите ответ и нажмите Enter: "); // Запрос ответа и отправка ответного сообщения
                            string replyMessage1 = "1234";
                            var replyMessageJson1 = new Message()
                            {
                                Date = DateTime.Now,
                                FromName = name,
                                Text = replyMessage1
                            }.ToJson();
                            byte[] replyBytes1 = Encoding.ASCII.GetBytes(replyMessageJson1);
                            udpClient.Send(replyBytes1, replyBytes1.Length, remoteEndPoint);
                        }
                        else if (message.Text == "1234")
                        {
                            Console.WriteLine("Сообщение дошло!!!!");


                        }



                        string replyMessage = Console.ReadLine();
                       mems = replyMessage;
                        if (mems=="Exit")
                        {
                           break;
                        }    
                        var replyMessageJson = new Message()
                        {
                            Date = DateTime.Now,
                            FromName = name,
                            Text = replyMessage
                        }.ToJson();
                        byte[] replyBytes = Encoding.ASCII.GetBytes(replyMessageJson);
                        udpClient.Send(replyBytes, replyBytes.Length, remoteEndPoint);
                        Console.WriteLine("Ответ отправлен.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                    }
                } 
            }
            ct.ThrowIfCancellationRequested();
        }
      
      

        static async Task Main(string[] args)
        {
           
            Task t = new Task(Server,ct);
            t.Start();
            while (true)
            {  string d = Console.ReadLine();
                if (d == "Exit")
                {
                    cts.Cancel();
                    break;
                        }
                 }
            Console.WriteLine("Задача завершена");
            try
            {
                await t;
            }
            catch(OperationCanceledException e)
            {
                if(t.IsCanceled)
                {
                    Console.WriteLine("Задача завершена");
                }
            }

                
           
        }
    }
}