using System;
using Microsoft.AspNetCore.Mvc;
using BankAPPWeb.Model;
using BankAPPWeb.Banks;
using Microsoft.AspNetCore.Http;

namespace BankAPPWeb.Controllers
{
    public class HomeController : Controller
    {
        Bank bank;
        public HomeController()
        {
            this.bank = new Bank();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Main([FromForm] int UserID, int PIN)
        {
            User item = this.bank.LoginUser(UserID, PIN);
            if (item.UserID == 0)
            {
                string er = "Incorrect UserID or PIN";
                this.HttpContext.Session.SetString("Error",er);
                var Err = this.HttpContext.Session.GetString("Error");
                ViewData["Error"] = Err;
                return View("Index");
            }
            else
            {
                //session-UserID
                this.HttpContext.Session.SetInt32("UserID", UserID);
                var UserIDOne = this.HttpContext.Session.GetInt32("UserID");
                ViewData["UserID1"] = UserIDOne;
                this.HttpContext.Session.SetInt32("PIN", PIN);
                var PINOne = this.HttpContext.Session.GetInt32("PIN");
                ViewData["UserID1"] = PINOne;
                //Cookie
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(30);
                option.SameSite = SameSiteMode.Strict;
                string UserID1 = Convert.ToString(UserID);
                Response.Cookies.Append("Cookie1", UserID1, option);
                return View("Main");
            }
        }
        [HttpGet]
        public IActionResult Display()
        {
            var IsCookieAvail = Request.Cookies.ContainsKey("Cookie1");
            string value;
            Request.Cookies.TryGetValue("Cookie1", out value);
            int item1 = this.bank.BalanceCheckUser(value);
            //
            this.HttpContext.Session.SetInt32("Balance", item1);
            var Bal = this.HttpContext.Session.GetInt32("Balance");
            ViewData["Balance"] = Bal;
            //
            return View("Display");
        }

        [HttpGet]
        public IActionResult DepositView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Deposit([FromForm] int Amt)
        {
            Request.Cookies.ContainsKey("Cookie1");
            Request.Cookies.TryGetValue("Cookie1", out string value);
            this.bank.DepositUser(Amt, value);
            //
            var UserIDOne = this.HttpContext.Session.GetInt32("UserID");
            ViewData["UserID1"] = UserIDOne;
            var PINOne = this.HttpContext.Session.GetInt32("PIN");
            ViewData["PIN"] = PINOne;
            //
            return View("DepositSuccess");
        }
        [HttpGet]
        public IActionResult WithdrawView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Withdraw([FromForm] int Amt)
        {
            var IsCookieAvail = Request.Cookies.ContainsKey("Cookie1");
            string value;
            Request.Cookies.TryGetValue("Cookie1", out value);
            int UID = Convert.ToInt32(value);
            int item1 = this.bank.BalanceCheckUser(value);
            //
            this.HttpContext.Session.SetInt32("Balance", item1);
            var Bal = this.HttpContext.Session.GetInt32("Balance");
            if (Amt <= Bal)
            {
                this.bank.WithdrawUser(Amt, UID);
                return View("WithdrawSuccess");
            }
            else
            {
                string st = "Low Balance";
                this.HttpContext.Session.SetString("st", st);
                var Err = this.HttpContext.Session.GetString("st");
                ViewData["st"] = Err;
                return View("WithdrawView");
            }
        }
        [HttpGet]
        public IActionResult TransferView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Transfer([FromForm] int Amt, int id2)
        {
            var IsCookieAvail = Request.Cookies.ContainsKey("Cookie1");
            string value;
            Request.Cookies.TryGetValue("Cookie1", out value);
            int UID = Convert.ToInt32(value);

            int item1 = this.bank.BalanceCheckUser(value);
            this.HttpContext.Session.SetInt32("Balance", item1);
            var Bal = this.HttpContext.Session.GetInt32("Balance");
            if (Amt <= Bal)
            {
                int item = this.bank.TransferUser(Amt, UID, id2);
                return View("TransferSuccess");
            }
            else {
                string st1 = "Low balance in your account!";
                this.HttpContext.Session.SetString("st1", st1);
                var stt1 = this.HttpContext.Session.GetString("st1");
                ViewData["st1"] = stt1;
                return View("TransferView");
            }
        }
        [HttpGet]
        public IActionResult Logouts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PincView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PinChange([FromForm] int pin1, int pin2,int pin3)
        {
            if (pin1 == pin2)
            {
                var IsCookieAvail = Request.Cookies.ContainsKey("Cookie1");
                string value;
                Request.Cookies.TryGetValue("Cookie1", out value);
                int UID = Convert.ToInt32(value);
                int item = this.bank.PinChangeUser(UID, pin3);
            }
            return View("PinCSuccess");
        }
        [HttpGet]
        public IActionResult TransLogView()
        {
            var IsCookieAvail = Request.Cookies.ContainsKey("Cookie1");
            string value;
            Request.Cookies.TryGetValue("Cookie1", out value);
            int UID = Convert.ToInt32(value);
            User[] Trans = this.bank.TransLog(UID);
            return View(Trans);
        }
    }
}
