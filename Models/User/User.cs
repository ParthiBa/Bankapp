using System.Collections.Generic;

namespace BankAPPWeb.Model
{
    public class User
    {
        public int PIN { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public int TransID { get; set; }

        public string CD { get; set; }

        public int Amount { get; set; }

        public int AccountNo { get; set; }

        public string Dated { get; set; }
    }
}