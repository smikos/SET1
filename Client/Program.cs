using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace sem1
{
    class Program
    {
        internal class Message
        {
            public string FromName { get; set; }
            public DateTime Date { get; set; }
            public string Text { get; set; }
            // Метод для сериализации в JSON
            public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
            // Статический метод для десериализации JSON в объект MyMessage
            public static Message FromJson(string json)
            {
                return JsonSerializer.Deserialize<Message>(json);
            }
        }

        static void Main(string[] args)
        {
            static void Client(string name, string ip)
            {
                // Инициализация объектов UdpClient и IPEndPoint
                UdpClient udpClient;
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
                udpClient = new UdpClient();
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
                while (true)
                {
                    try
                    {
                        // Запрос на ввод сообщения и отправка его на сервер
                        Console.WriteLine("UDP Клиент ожидает ввода сообщения");
                        Console.Write("Введите сообщение и нажмите Enter: ");
                        string message = Console.ReadLine();
                        var messageJson = new Message() { Date = DateTime.Now, FromName = name, Text = message }.ToJson();
                        byte[] replyBytes = Encoding.ASCII.GetBytes(messageJson);
                        if (message != "Exit")
                        {
                            udpClient.Send(replyBytes, replyBytes.Length, remoteEndPoint);
                            Console.WriteLine("Сообщение отправлено.");
                        }
                        else { break; }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                    }
                    // Прием и вывод ответа от сервера
                    byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    var messageReceived = Message.FromJson(receivedData);
                    if (messageReceived.Text != "1234")
                    {
                        Console.WriteLine($"Получено сообщение от {messageReceived.FromName} ({messageReceived.Date}):");
                        Console.WriteLine(messageReceived.Text);
                        string message = "1234";
                        var messageJson = new Message() { Date = DateTime.Now, FromName = name, Text = message }.ToJson();
                        byte[] replyBytes = Encoding.ASCII.GetBytes(messageJson);
                        udpClient.Send(replyBytes, replyBytes.Length, remoteEndPoint);

                    }
                    else if (messageReceived.Text == "1234")
                    {
                        Console.WriteLine("Сообщение дошло!!!!");
                    }


                }
            }
            Client("CLient", "127.0.0.1");
        }
    }
}