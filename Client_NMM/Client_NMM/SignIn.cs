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

namespace Client_NMM
{
    public partial class SignIn : Form
    {
        public SignIn()
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

                    if (mess.Contains("SignIn:"))
                    {
                        mess = mess.Replace("SignIn", string.Empty);
                        if (mess.Contains("user đã tồn tại"))
                        {
                            MessageBox.Show("User đã tồn tại!");
                            return;
                        }
                        else if (mess.Contains("đăng ký thành công"))
                        {
                            MessageBox.Show("Đăng ký thành công!");
                            return;
                        }
                        else if (mess.Contains("đăng ký không thành công"))
                        {
                            MessageBox.Show("Đăng ký không thành công!");
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
            }
            catch
            {
                MessageBox.Show("Không thể kết nối tới server!");
            }

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void signB_Click(object sender, EventArgs e)
        {
            string username = nameTB.Text;
            string password = passTB.Text;
            string repeat = repeatTB.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeat))
            {
                MessageBox.Show("Please enter username, password and repeat password.");
                return;
            }

            if (password != repeat)
            {
                MessageBox.Show("Password and repeat password do not match!");
                return;
            }

            connect();
            SendMessage("SignIn:" + username + " " + password + "\n");
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
        }
    }
}
