using System;
using System.Data;
//using MySql.Data.MySqlClient;
using BankAPPWeb.Model;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace BankAPPWeb.accountDAO
{
    public class AccountDAO : IAccountDAO
    {
        //MySqlConnection conn;
        //string myConnectionString;
        private readonly SqliteConnection conn;
        private readonly ILogger logger;
        public AccountDAO(ILoggerFactory loggerFactory)
        {
            /*IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
            var section = Configuration.GetSection("ConnectionStrings");
            myConnectionString = section.Value;
            conn = new MySqlConnection();
            conn.ConnectionString = myConnectionString;
            */
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder(); ;
            connectionStringBuilder.DataSource = "./BankAPP_Data.db";
            connectionStringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            conn = new SqliteConnection(connectionStringBuilder.ConnectionString);
            this.logger = loggerFactory.CreateLogger(typeof(AccountDAO));
        }
        public User Login(int UserID, int PIN)
        {
            /*conn.Open();
            string selectloginQuery = " SELECT  UserID, UserName  FROM Customers where UserID = " + UserID + " AND PIN = " + PIN;
            MySqlCommand view = new MySqlCommand(selectloginQuery, conn);
            MySqlDataReader dr = view.ExecuteReader();
            User user1 = new User();
            while (dr.Read())
            {
                user1.UserID = dr.GetInt32(0);
                user1.UserName = dr.GetString(1);
            }
            if (user1.UserName == null)
            {
                return null;
            }
            conn.Close();
            return user1;
            */
            string selectloginQuery = " SELECT  UserID, UserName  FROM Customers where UserID = " + UserID + " AND PIN = " + PIN;
            SqliteCommand view = new SqliteCommand(selectloginQuery, conn);
            logger.LogError($"Connection which has been used {conn.ConnectionString}");
            conn.Open();
            SqliteDataReader dr = view.ExecuteReader();
            User user1 = new User();
            try
             {
                while (dr.Read())
                {
                    user1.UserID = dr.GetInt32(0);
                }

                return user1;
            }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 user1.UserID = 0;
                 return user1;
             }
             finally
             {
                 conn.Close();
             }
            
        }
        public int BalanceCheck(String UserID)
        {
            conn.Open();
            string BalanceCheckQuery = " SELECT TotAmount FROM Bank where UserID = " + UserID;
            SqliteCommand view = new SqliteCommand(BalanceCheckQuery, conn);
            SqliteDataReader reader = view.ExecuteReader();
            int[] Balance = new int[1];
            while (reader.Read())
            {
                Balance[0] = reader.GetInt32(0);
            }
            conn.Close();
            return Balance[0];
        }
        public int Deposit(int DepositAmount,string value)
        {
            string selectBankQuery = "SELECT TotAmount FROM Bank WHERE UserID =" + value;
            SqliteCommand view = new SqliteCommand(selectBankQuery, conn);
            conn.Open();
            SqliteDataReader reader = view.ExecuteReader();
            if (reader.Read())
            {
                int[] TotAmount = new int[1];
                TotAmount[0] = reader.GetInt32(0);
                TotAmount[0] = TotAmount[0] + DepositAmount;
                conn.Close();
                UpdateAmount(Convert.ToInt32(value), TotAmount[0]);
                InsertDepositTrans(Convert.ToInt32(value), DepositAmount);
            }
            return DepositAmount;
        }

        public int PINChange(int UserID, int NewPIN)
        {
            string NewPINChangeQuery = "UPDATE  Customers SET Pin=" + NewPIN + " where UserID = " + UserID;
            SqliteCommand updateCommand = new SqliteCommand(NewPINChangeQuery, conn);
            conn.Open();
            SqliteDataReader reader = updateCommand.ExecuteReader();
            conn.Close();
            return NewPIN;
        }
        public int Withdraw(int WithdrawAmount,int UID)
        {
            string selectBankQuery = "SELECT TotAmount FROM Bank WHERE UserID =" + UID;
            SqliteCommand view = new SqliteCommand(selectBankQuery, conn);
            conn.Open();
            SqliteDataReader reader = view.ExecuteReader();
            if (reader.Read())
            {
                int[] TotAmount = new int[1];
                TotAmount[0] = reader.GetInt32(0);
                conn.Close();
                if (WithdrawAmount <= TotAmount[0])
                {
                    TotAmount[0] = TotAmount[0] - WithdrawAmount;
                    Console.WriteLine(TotAmount[0]);
                    UpdateAmount(UID, TotAmount[0]);
                    InsertWithdrawTrans(UID,WithdrawAmount);

                }
            }
            return WithdrawAmount;
        }

        public void UpdateAmount(int UserID, int TotAmount)
        {
            string UpdateQuery = "UPDATE  Bank SET TotAmount =" + TotAmount + " where UserID = " + UserID;
            SqliteCommand updateCommand = new SqliteCommand(UpdateQuery, conn);
            conn.Open();
            updateCommand.ExecuteNonQuery();
            conn.Close();
        }

        public void InsertDepositTrans(int UserID, int TotAmount)
        {
            string selectTransQuery = "SELECT AccountNo from Trans Where UserID=" + UserID;
            SqliteCommand view = new SqliteCommand(selectTransQuery, conn);
            conn.Open();
            SqliteDataReader reader = view.ExecuteReader();
            int[] AccountNo = new int[1];
            if (reader.Read())
            {
                AccountNo[0] = reader.GetInt32(0);
            }
            conn.Close();
            string InsertTransQuery = "INSERT INTO Trans (CD,Amount,AccountNo,UserID,Dated) VALUES ('C'," + TotAmount + "," + AccountNo[0] + "," + UserID + ",@DATE)";
            SqliteCommand updateCommand = new SqliteCommand(InsertTransQuery, conn);
            updateCommand.Parameters.AddWithValue("@DATE", DateTime.Now);
            conn.Open();
            int RowCount = updateCommand.ExecuteNonQuery();
            conn.Close();
        }

        public void InsertWithdrawTrans(int UserID, int TotAmount)
        {
            string selectTransQuery = "SELECT AccountNo from Trans Where UserID=" + UserID;
            SqliteCommand view = new SqliteCommand(selectTransQuery, conn);
            conn.Open();
            SqliteDataReader reader = view.ExecuteReader();
            int[] AccountNo = new int[1];
            if (reader.Read())
            {
                AccountNo[0] = reader.GetInt32(0);
            }
            conn.Close();
            string InsertTransQuery = "INSERT INTO Trans (CD,Amount,AccountNo,UserID,Dated) VALUES ('D'," + TotAmount + "," + AccountNo[0] + "," + UserID + ",@DATE)";
            SqliteCommand updateCommand = new SqliteCommand(InsertTransQuery, conn);
            updateCommand.Parameters.AddWithValue("@DATE", DateTime.Now);
            conn.Open();
            int RowCount = updateCommand.ExecuteNonQuery();
            conn.Close();
        }
        public User[] TransLog(int UserID)
        {
            int i = 0;
            string countTransQuery = "SELECT COUNT(*) FROM Trans WHERE UserID = " + UserID;
            SqliteCommand countCommand = new SqliteCommand(countTransQuery, conn);
            conn.Open();
            Int64 n = (Int64)countCommand.ExecuteScalar();
            User[] Tran1 = new User[n];
            string TransLogQuery = "SELECT TransID, CD, Amount, AccountNo, UserID, Dated FROM Trans WHERE UserID = " + UserID;
            SqliteCommand selectCommand = new SqliteCommand(TransLogQuery, conn);
            SqliteDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                User Tran = new User();
                Tran.TransID = reader.GetInt32(0);
                Tran.CD = reader.GetString(1);
                Tran.Amount = reader.GetInt32(2);
                Tran.AccountNo = reader.GetInt32(3);
                Tran.Dated = reader.GetString(5);
                Tran1[i] = Tran;
                i++;
            }
            conn.Close();
            return Tran1;
        }
    }
}