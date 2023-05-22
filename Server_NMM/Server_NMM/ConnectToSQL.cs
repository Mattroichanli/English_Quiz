using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Server_NMM
{
    class ConnectToSQL
    {
        SqlConnection connect = null;
        DataTable dt;
        string connectionString = @"Data Source=DESKTOP-7UG7M23;Initial Catalog=EnglishQuiz;Integrated Security=True";

        public SqlConnection OpenConnect()
        {
            if (connect == null)
                connect = new SqlConnection(connectionString);
            if (connect.State == ConnectionState.Closed)
                connect.Open();
            return connect;
        }
        public SqlConnection CloseConnect()
        {
            connect = new SqlConnection(connectionString);
            if (connect.State == ConnectionState.Open)
                connect.Close();
            return connect;
        }

        public string UserLog(string user, string password)
        {
            OpenConnect();
            SqlCommand command = new SqlCommand("sp_LogIn", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar)).Value = user;
            SqlDataReader read = command.ExecuteReader();

            string re ="";
            if (!read.HasRows) 
                re = "invalid user";
            else
            {
                read.Read();
                if (user == read.GetString(0))
                {
                    if (read.GetString(1) == password) re = "success";
                    else re = "wrong password";
                }
            }

            read.Close();
            CloseConnect();
            return re;
        }

        public string UserSign(string user, string password)
        {
            string re = "";
            string x = UserLog(user, password);
            if (x == "success" || x == "wrong password")
                re = "user đã tồn tại";
            else
            {
                OpenConnect();
                SqlCommand command = new SqlCommand("sp_SignIn", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar)).Value = user;
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar)).Value = password;
                if (command.ExecuteNonQuery() == 1)
                    re = "đăng ký thành công";
                else re = "đăng ký không thành công";
            }
            CloseConnect();
            return re;
        }
        public DataTable RandomWord(string unit)
        {
            OpenConnect();

            SqlCommand command = new SqlCommand("sp_Unit", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CHUDE", SqlDbType.VarChar)).Value = unit;
            string MACD = command.ExecuteScalar().ToString();

            SqlCommand cmd = new SqlCommand("sp_RandomWord", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MACD", SqlDbType.VarChar)).Value = MACD;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            dt = new DataTable();
            adapter.Fill(dt);

            CloseConnect();
            return dt;
        }
    }
}
