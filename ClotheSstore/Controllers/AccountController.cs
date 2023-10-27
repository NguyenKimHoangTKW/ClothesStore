using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
        public ActionResult Login(Customer cus, UserAdmin ad)
        {
            var fpassworduser = GetMD5(cus.passWord);
            var fpasswordadmin = GetMD5(ad.passWord);
            var isCustomer = db.Customers.SingleOrDefault(c => c.userName == cus.userName && c.passWord == fpassworduser);
            var isAdmin = db.UserAdmins.SingleOrDefault(a => a.userName == ad.userName && a.passWord == fpasswordadmin);

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
                var password = GetMD5(f["Password"]);
                var checkpassword = GetMD5(f["CheckPassword"]);
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

                customer.passWord = GetMD5( password);
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
        public ActionResult TitleUser(int? id)
        {
            var titlecus = from cus in db.Customers
                           where cus.idCustomer == id
                           select cus;
            return View(titlecus);
        }
        [HttpGet]
        public ActionResult EditTitleUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTitleUser([Bind(Include = "idCustomer,codeCustomer,nameCustomer,userName,passWord,birthDay,email,address,phone,credate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ThongBao = "Thay đổi thông tin thành công";
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(customer);
        }
        [HttpGet]
        public ActionResult EditPassWordUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassWordUser([Bind(Include = "idCustomer,codeCustomer,nameCustomer,userName,passWord,birthDay,email,address,phone,credate")] Customer customer, FormCollection f)
        {
            
            if (ModelState.IsValid)
            {
                var sPassword = GetMD5(f["PassWord"]);
                var sCheckPassWord = GetMD5(f["CheckPassword"]);
                if(sPassword != sCheckPassWord)
                {
                    ViewBag.ThongBao = "Mật khẩu nhập lại không chính xác";
                }
                else
                {
                    customer.passWord = sPassword;
                    ViewBag.ThongBao = "Thay đổi mật khẩu thành công";
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                } 
            }
            return View(customer);
        }
        public ActionResult Error()
        {
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}