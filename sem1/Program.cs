using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace sem1
{
    class Program
    {
        static void Server(string name)
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
                   else if(message.Text == "1234")
                    {
                        Console.WriteLine("Сообщение дошло!!!!");


                    }
                  

                    
                    string replyMessage = Console.ReadLine();
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
      
      

        static void Main(string[] args)
        {/*
            Console.WriteLine("Enter some Integer");
            if (args.Length == 1)
            {
                // Если указан только один аргумент, запускается сервер
                Server("d");
            }
            else if (args.Length == 2)
            {
                // Если указано два аргумента, запускается клиент
               Client(args[0], args[1]);
            }
            else
            {
                // Если указано неверное количество аргументов, выводится информация о запуске
                Console.WriteLine("Для запуска сервера введите ник-нейм как параметр запуска приложения");
                Console.WriteLine("Для запуска клиента введите ник-нейм и IP сервера как параметры запуска приложения");
            }
            */
           Server("Сервер");
            //Client("DOSD", "127.0.0.1");
        }
    }
}