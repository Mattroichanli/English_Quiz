using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_NMM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpClient tcpClient;
        NetworkStream ns;
        string ip = "127.0.0.1";
        int port = 8080;
        int diem = 0;

        public string CorrectAnswer;

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
        public void RandomQuiz(string[] source, int i)
        {
            //MessageBox.Show("RandomQuiz");
            if (source.Length == 6)
            this.Invoke((MethodInvoker)(() =>
            {
                label4.Text = (i+1).ToString() + "/10";
                radioBtn_A.Checked = false;
                radioBtn_B.Checked = false;
                radioBtn_C.Checked = false;
                radioBtn_D.Checked = false;

                label_Cauhoi.Text = source[0].Trim();
                radioBtn_A.Text = source[1].Trim();
                radioBtn_B.Text = source[2].Trim();
                radioBtn_C.Text = source[3].Trim();
                radioBtn_D.Text = source[4].Trim();
                CorrectAnswer = source[5].Trim();
            }));
        }
        public void ShowQuestion(string[] question, int i, int diem)
        {
            //MessageBox.Show("Show Question " + i.ToString());
            if( i < question.Length)
            {
                //MessageBox.Show("Show Question If " + i.ToString());
                string[] source = question[i].Split(';');
                //MessageBox.Show("Tới đây rồi nè");
                string Answer = "";
                RandomQuiz(source, i);
                while (radioBtn_A.Checked == false &&
                       radioBtn_B.Checked == false &&
                       radioBtn_C.Checked == false &&
                       radioBtn_D.Checked == false)
                {
                    Application.DoEvents();
                }
                this.Invoke((MethodInvoker)(() =>
                    {
                        //MessageBox.Show("in while");
                        if (radioBtn_A.Checked)
                        {
                            Answer = radioBtn_A.Text;
                            //MessageBox.Show("Bạn đã chọn " + Answer);
                        }
                        else if (radioBtn_B.Checked)
                        {
                            Answer = radioBtn_B.Text;
                            //MessageBox.Show("Bạn đã chọn " + Answer);
                        }
                        else if (radioBtn_C.Checked)
                        {
                            Answer = radioBtn_C.Text;
                            //MessageBox.Show("Bạn đã chọn " + Answer);
                        }
                        else if (radioBtn_D.Checked)
                        {
                            Answer = radioBtn_D.Text;
                            //MessageBox.Show("Bạn đã chọn " + Answer);
                        }
                    }));
                
                if (Answer == CorrectAnswer)
                {
                    MessageBox.Show("Chính xác!");
                    diem++;
                    this.Invoke((MethodInvoker)(() =>
                    {
                        label_socaudung.Text = diem.ToString();
                    }));
                }
                else MessageBox.Show("Chưa chính xác! \nBạn đã chọn " + Answer + "\n Đáp án đúng là: " + CorrectAnswer);

                // Chạy lại phương thức đệ quy để hiển thị câu hỏi tiếp theo
                ShowQuestion(question, i + 1, diem);
            }
            return;
        }

        public void ReceiveMessages()
        {
            string mess = string.Empty;
            try
            {
                while (true)
                {
                    ns = tcpClient.GetStream();
                    var bufferSize = tcpClient.ReceiveBufferSize;
                    byte[] data = new byte[bufferSize];
                    ns.Read(data, 0, data.Length);
                    mess = Encoding.Unicode.GetString(data);

                    if (mess.Contains("RandomQuiz:"))
                    {
                        mess = mess.Replace("RandomQuiz:", string.Empty);
                        //MessageBox.Show(mess);
                        string[] question = mess.Split('\n');
                        //MessageBox.Show("question.length: " + question.Length.ToString());
                        ShowQuestion(question, 0, 0);
                    }
                    //else if (mess.Contains("EndGame"))
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
        private void btn_play_Click(object sender, EventArgs e)
        {
            connect();
            SendMessage("RandomQuiz:" + comboBox1.Text + '\n');
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}