using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClotheSstore.Controllers
{
    public class AccountController : Controller
    {
        
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer cus, Admin ad)
        {
            var isCustomer = db.Customers.SingleOrDefault(c => c.userName == cus.userName && c.passWord == cus.passWord);
            var isAdmin = db.Admins.SingleOrDefault(a => a.userName == ad.userName && a.passWord == ad.passWord);
            if (isCustomer != null)
            {
                Session["Customer"] = isCustomer;
                return RedirectToAction("Index", "Home");
            }
            else if (isAdmin != null)
            {
                Session["Admin"] = isAdmin;
                return RedirectToAction("Index", "Home", new {area = "Admin"});
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}