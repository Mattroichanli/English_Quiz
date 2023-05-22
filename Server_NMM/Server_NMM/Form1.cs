using System;
using System.Collections;
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
using System.Xml;

namespace Server_NMM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ArrayList connections = new ArrayList(100);     //danh sach cac ket noi cua client
        private ArrayList networkStreams = new ArrayList(100);  //danh sach cac NetworkStream duoc tao
        //string ip = "192.168.164.26";
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        Socket listenerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
        


        public void createNewListenThread()
        {
            Thread newThread = new Thread(new ThreadStart(Listener));
            //newThread.IsBackground = true;
            newThread.Start();
        }

        public void Listener()
        {
            int bytesReceived = 0;
            byte[] recv = new byte[2];

            try
            {
                Socket clientSocket = listenerSocket.Accept();
                NetworkStream ns = new NetworkStream(clientSocket);
                connections.Add(clientSocket);
                networkStreams.Add(ns);

                var endPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                //MessageBox.Show("aaaa1");
                while (clientSocket.Connected)
                {
                    string text = "";

                    do
                    {
                        bytesReceived = clientSocket.Receive(recv);
                        text += Encoding.Unicode.GetString(recv);
                    }
                    while (text[text.Length - 1] != '\n');
                    //MessageBox.Show("aaaa");
                    if (text.Contains("LogIn:"))
                    {
                        text = text.Replace("LogIn:", string.Empty);
                        text = text.Replace("\n", string.Empty);
                        string[] s = text.Split(' ');
                        SendMessage(LogIn(s[0], s[1]), clientSocket);
                    }
                    else if (text.Contains("RandomQuiz:")) //Random Quiz
                    {
                        text = text.Replace("RandomQuiz:", string.Empty);
                        SendMessage(RandomQuiz(text.Trim()), clientSocket);
                    }
                    else if (text.Contains("SignIn:"))
                    {
                         //MessageBox.Show("bbbb");
                         text = text.Replace("SignIn:", string.Empty);
                         text = text.Replace("\n", string.Empty);
                         string[] s = text.Split(' ');
                         SendMessage(SignIn(s[0], s[1]), clientSocket);
                    }
                    else if (text == "quit\n")
                    {
                        connections.Remove(clientSocket);
                        clientSocket.Close();
                    }
                }
                connections.Remove(clientSocket);
                networkStreams.Remove(ns);
                ns.Close();
                clientSocket.Close();
                if (listenerSocket.Connected)
                        createNewListenThread();
            }
            catch
            {
                if (listenerSocket.Connected)
                    createNewListenThread();
            }
        }
        void SendMessage(string message, Socket client)
        {
            if (client != null)
            client.Send(Encoding.Unicode.GetBytes(message));
            textBox1.Text = message;
            //MessageBox.Show("Server đã gửi mess: " + message);
        }

        string logResult;
        private string LogIn(string username, string password)
        {
            ConnectToSQL cts = new ConnectToSQL();
            logResult = cts.UserLog(username, password);
            string s = "LogIn:" + logResult;
            return s;
        }
        string signResult;
        private string SignIn(string username, string password)
        {            
            ConnectToSQL cts = new ConnectToSQL();
            signResult = cts.UserSign(username, password);
            return "SignIn:" + signResult;
        }

        DataTable dt;
        private string RandomQuiz(string unit)
        {
            ConnectToSQL cts = new ConnectToSQL();
            dt = new DataTable();
            dt = cts.RandomWord(unit);
            int n = dt.Rows.Count;
            string message = "RandomQuiz:";
            Random randomNumber = new Random();

            for (int i = 0; i < 10; i++)
            {
                int randomRow = randomNumber.Next(n); //Random từ 0 đến n-1
                DataRow row = dt.Rows[randomRow];

                    message +=
                    row["CAUHOI"].ToString() + ';' +
                    row["DAPAN_A"].ToString() + ';' +
                    row["DAPAN_B"].ToString() + ';' +
                    row["DAPAN_C"].ToString() + ';' +
                    row["DAPAN_D"].ToString() + ';' +
                    row["DAPAN_DUNG"].ToString() + '\n';
            }
            return message.Substring(0,message.Length-1);
        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            listenerSocket.Bind(iPEndPoint);
            listenerSocket.Listen(-1);
            CheckForIllegalCrossThreadCalls = false;

            for (int i = 0; i < 100; i++)
            {
                createNewListenThread();
            }
            btn_listen.Enabled = false; 
        }
    }
}
