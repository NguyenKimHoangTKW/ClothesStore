using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer cus, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                cus.idCustomer = nextId;
                cus.codeCustomer = "KH" + nextId.ToString("2023BS");
                cus.credate = DateTime.Now;
                var password = f["Password"];
                var checkpassword = f["CheckPassword"];
                if (db.Customers.SingleOrDefault(c => c.userName == cus.userName) != null)
                {
                    ViewBag.ThongBao = "Tài khoản đã tồn tại, vui lòng nhập tài khoản khác";
                   
                }
                else if (db.Customers.SingleOrDefault(c => c.email == cus.email) != null)
                {
                    ViewBag.ThongBao = "Email này đã tồn tại, vui lòng nhập Email khác";
                }
                else if (password != checkpassword)
                {
                    ViewBag.ThongBao = "Mật khẩu nhập lại không đúng";
                }
                else
                {
                    cus.passWord = password;
                    ViewBag.ThongBao = "Đăng ký thành công";
                    db.Customers.Add(cus);
                    db.SaveChanges();

                }
            }

            return View();
        }
        private int GetNextId()
        {
            int? maxId = db.Customers.Max(t => (int?)t.idCustomer);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string username, string password, string confirmPassword)
        {
            var customer = db.Customers.SingleOrDefault(c => c.email == username);

            if (customer != null)
            {
                if (password != confirmPassword)
                {
                    ViewBag.ThongBao = "Mật khẩu và mật khẩu xác nhận không khớp.";
                    return View();
                }

                customer.passWord = password;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.ThongBao = "Mật khẩu đã được thay đổi thành công.";
            }
            else
            {
                ViewBag.ThongBao = "Không tìm thấy khách hàng với email này.";
               
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