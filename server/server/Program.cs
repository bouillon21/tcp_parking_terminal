using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static User[] user;

        static byte[] handler(string request, int places)
        {
            if (request.IndexOf("del") == 0)
            {
                string[] words = request.Split(new char[] { '@' });

                Console.WriteLine(words[1]);

                for (int i = 0; i < places; i++)
                {
                    if (user[i] != null && user[i].id == Convert.ToInt32(words[1]))
                    {
                        user[i] = null;
                        return (Encoding.UTF8.GetBytes("удалил" + words[1]));
                    }
                }
            }
            if (request.IndexOf("price") == 0)
            {
                string[] words = request.Split(new char[] { '@' });

                Console.WriteLine(words[1]);

                for (int i = 0; i < places; i++)
                {
                    if (user[i] != null && user[i].id == Convert.ToInt32(words[1]))
                    {
                        return (Encoding.UTF8.GetBytes(user[i].parking_price(DateTime.Now, 55)));
                    }
                }
            }
            switch (request)
            {
                case "free":
                    {
                        int free = 0;

                        for (int i = 0; i < places; i++)
                        {
                            if (user[i] == null)
                                free++;
                        }
                        return (Encoding.UTF8.GetBytes(free.ToString()));
                    }
                case "add":
                    {
                        Random rand = new Random();
                        int id = 0;

                        for (int i = 0; i < places; i++)
                        {
                            if (user[i] == null)
                            {
                                id = rand.Next(1000, 9999);
                                user[i] = new User(id, DateTime.Now);
                                return (Encoding.UTF8.GetBytes(id.ToString()));
                            }
                        }
                        return (Encoding.UTF8.GetBytes(id.ToString()));
                    }
            }
            return (Encoding.UTF8.GetBytes(""));
        }

        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener server = new TcpListener(localAddr, port);
            int places;

            Console.WriteLine("Сервер запущен!");
            Console.WriteLine("Введите колличество свободных мест на парковке!");
            places = Convert.ToInt32(Console.ReadLine());
            user = new User[places];

            server.Start();
            Console.WriteLine("Сервер работает!");

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
                            Byte[] responseData = handler(myCompleteMessage.ToString(), places);
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
