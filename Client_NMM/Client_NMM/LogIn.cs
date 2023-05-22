using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Activation;

namespace Client_NMM
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        TcpClient tcpClient;
        NetworkStream ns;
        string ip = "127.0.0.1";
        int port = 8080;

        public void ReceiveMessages()
        {
            string mess;
            try
            {
                while (true)
                {
                    ns = tcpClient.GetStream();
                    var bufferSize = tcpClient.ReceiveBufferSize;
                    byte[] data = new byte[bufferSize];
                    ns.Read(data, 0, data.Length);
                    mess = Encoding.Unicode.GetString(data);

                    if (mess.Contains("LogIn:"))
                    {
                        mess = mess.Replace("LogIn:", string.Empty);
                        //MessageBox.Show(mess);
                        if (mess.Contains("success"))
                        {
                            MessageBox.Show("Đăng nhập thành công!");
                            Form form1 = new Form1();
                            form1.ShowDialog();
                        }
                        else if (mess.Contains("wrong password"))
                        {
                            MessageBox.Show("Mật khẩu không đúng!");
                            return;
                        }
                        else if (mess.Contains("invalid user"))
                        {
                            MessageBox.Show("Tài khoản không tồn tại!");
                            return;
                        }
                    }
                }
            }
            catch
            {
                tcpClient.Close();
            }
        }

        public void SendMessage(string message)
        {
            Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
            ns.Write(data, 0, data.Length);
            //MessageBox.Show("Client đã gửi mess " + message);
        }

        public void connect()
        {
            tcpClient = new TcpClient();

            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            try
            {
                tcpClient.Connect(iPEndPoint);
                ns = tcpClient.GetStream();
                //MessageBox.Show("Đã kết nối tới server!");
            }
            catch
            {
                MessageBox.Show("Không thể kết nối tới server!");
            }

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void logB_Click(object sender, EventArgs e)
        {
            connect();

            string username = nameTB.Text;
            string password = passTB.Text;


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            SendMessage("LogIn:" + username + " " + password + "\n");
            return;
        }

        private void signB_Click(object sender, EventArgs e)
        {
            Form signInForm = new SignIn();
            this.Hide();
            signInForm.ShowDialog();
            this.Show();
        }
    }
}
