using System;
using BankAPPWeb.accountDAO;
using BankAPPWeb.Model;

namespace BankAPPWeb.Banks
{
    public class Bank
    {
        public int UserID = 0, PIN = 0, UserName = 0, UserID2 = 0, ToAmount = 0;
        AccountDAO accountdao;
        public Bank(AccountDAO accountDAO)
        {
            this.accountdao = accountDAO;
        }
        public User LoginUser(int UserID, int PIN)
        {
            User User1 = this.accountdao.Login(UserID, PIN);
            return User1;
        }
        public int BalanceCheckUser(String UserID)
        {
            int User2 = this.accountdao.BalanceCheck(UserID);
            return User2;
        }
        public int DepositUser(int Amt, string value)
        {
            int User2 = this.accountdao.Deposit(Amt, value);
            return User2;
        }
        public int WithdrawUser(int Amt, int UID)
        {
            int User3 = this.accountdao.Withdraw(Amt, UID);
            return User3;
        }
        public int TransferUser(int Amt, int UID, int UID2)
        {
            int User3 = this.accountdao.Withdraw(Amt, UID);
            int User4 = this.accountdao.Deposit(Amt, Convert.ToString(UID2));
            return User3;
        }
        public int PinChangeUser(int UID, int NewPIN)
        {
            int User = this.accountdao.PINChange(UID, NewPIN);
            return User;
        }
        public User[] TransLog(int UID)
        {
            User[] Trans = this.accountdao.TransLog(UID);
            return Trans;
        }
    }
}