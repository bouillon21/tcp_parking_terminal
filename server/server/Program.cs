using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static int places;
        static int id;
        static byte[] handler(string request)
        {
            switch (request)
            {
                case "free":
                        return (Encoding.UTF8.GetBytes(places.ToString()));
                    break;
                case "-":
                    {
                        places++;
                        return (Encoding.UTF8.GetBytes(places.ToString()));
                    }
                    break;
                case "+":
                    {
                        if (places == 0)
                            return (Encoding.UTF8.GetBytes("мест нет"));
                        places--;
                        return (Encoding.UTF8.GetBytes(""));
                    }
                    break;
                case "id":
                    {
                        if (places > 0)
                        {
                            id++;
                        }
                            return (Encoding.UTF8.GetBytes(id.ToString()));
                    }
                default:
                    return (Encoding.UTF8.GetBytes("ERROR!"));
                    break;
            }
        }

        static void Main(string[] args)
        {
            places = 3;

            Console.WriteLine("Сервер запущен!");

            // Инициализация
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener server = new TcpListener(localAddr, port);
            // Запуск в работу
            server.Start();
            // Бесконечный цикл
            while (true)
            {
                try
                {
                    // Подключение клиента
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            StringBuilder myCompleteMessage = new StringBuilder();
                            int numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                            }
                            while (stream.DataAvailable);
                            Byte[] responseData = handler(myCompleteMessage.ToString());
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    finally
                    {
                        stream.Close();
                        client.Close();
                    }
                }
                catch
                {
                    server.Stop();
                    break;
                }
            }
        }
    }
}
