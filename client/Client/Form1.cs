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
using System.Net.Mail;
using System.Net;

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
                MessageBox.Show("Предоставте к оплате " + (Exchange("127.0.0.1", 8888, "price@"+ id) + " рублей"));
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
            string id = Exchange("127.0.0.1", 8888, "add");
            try
            {
                MailAddress from = new MailAddress("4232g@mail.ru", "Parking");
                MailAddress to = new MailAddress(textBox2.Text);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Чек";
                m.Body = "Ваш id на парковке " + id;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential("4232g@mail.ru", "Qwe123asd45");
                smtp.EnableSsl = true;
                smtp.Send(m);
                MessageBox.Show($"Чек отправлен на почту {textBox2.Text}");
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка во время отправки");
                Application.Exit();
            }
            MessageBox.Show("Ваш id " + id + "\nЗапомните его для оплаты парковочного места");
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
