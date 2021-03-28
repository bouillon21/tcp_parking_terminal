using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool flag1 = false;

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
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Exchange("127.0.0.1", 8888, "free"));
        }
      
    
        private void button3_Click(object sender, EventArgs e)
        {
            string id;
            id = textBox1.Text;
            if (Convert.ToInt32(textBox1.Text) < 9999)
            {
                MessageBox.Show("Предоставте к оплате " + (Exchange("127.0.0.1", 8888, "price@"+ id) + "рублей"));
            }
            else 
            {
                MessageBox.Show("Некорректный ввод id");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
               MessageBox.Show("Ваш id " + Exchange("127.0.0.1", 8888, "add") + "\nЗапомните его для оплаты парковочного места");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string id;
            id = textBox1.Text;
            if (Convert.ToInt32(textBox1.Text) < 9999)
            {
                Exchange("127.0.0.1", 8888, "del@" + id);
                MessageBox.Show("Оплачено \n Счастливого вам пути!");
            }
            else
            {
                MessageBox.Show("Некорректный ввод id");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
