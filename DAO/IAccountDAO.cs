using System;
using System.Data;
//using MySql.Data.MySqlClient;
using BankAPPWeb.Model;

namespace BankAPPWeb.accountDAO
{
    public interface IAccountDAO
    {
        public User Login(int UserID, int PIN);
        public int BalanceCheck(String UserID);
        public int Deposit(int DepositAmount,string value);
        public int PINChange(int UserID, int NewPIN);
        public int Withdraw(int WithdrawAmount,int UID);

        public void UpdateAmount(int UserID, int TotAmount);

        public void InsertDepositTrans(int UserID, int TotAmount);

        public void InsertWithdrawTrans(int UserID, int TotAmount);
        public User[] TransLog(int UserID);
    }
}