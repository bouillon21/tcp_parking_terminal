﻿using System;
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
                    case "free":
                        Console.WriteLine("Колличество мест:    " + Exchange("127.0.0.1", 8888, "free"));
                    break;
                    case "+":
                        {
                            Console.WriteLine(Exchange("127.0.0.1", 8888, "+"));
                            Console.WriteLine("ваш id:  " + Exchange("127.0.0.1", 8888, "id"));
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