using System;
using System.Net.Sockets;
using System.Text;

public class Class1
{
	public Class1()
	{
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
