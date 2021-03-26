using System;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "add":
                        {
                            Console.WriteLine(Exchange("127.0.0.1", 8888, "add"));
                        }
                        break;
                    case "free":
                        {
                            Console.WriteLine(Exchange("127.0.0.1", 8888, "free"));
                        }
                        break;
                    case "del":
                        {
                            string id;
                            Console.WriteLine("введите id");
                            id = Console.ReadLine();
                            Console.WriteLine(Exchange("127.0.0.1", 8888, "del@"+id));
                        }
                        break;
                    case "price":
                        {
                            string id;
                            Console.WriteLine("введите id");
                            id = Console.ReadLine();
                            Console.WriteLine(Exchange("127.0.0.1", 8888, "price@" + id));
                        }
                        break;
                }
            }
        }

        static private string Exchange(string address, int port, string outMessage)
        {
            // Инициализация
            TcpClient client = new TcpClient(address, port);
            Byte[] data = Encoding.UTF8.GetBytes(outMessage);
            NetworkStream stream = client.GetStream();
            try
            {
                // Отправка сообщения
                stream.Write(data, 0, data.Length);
                // Получение ответа
                Byte[] readingData = new Byte[256];
                String responseData = String.Empty;
                StringBuilder completeMessage = new StringBuilder();
                int numberOfBytesRead = 0;
                do
                {
                    numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
                    completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
                }
                while (stream.DataAvailable);
                    responseData = completeMessage.ToString();
                return responseData;
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
