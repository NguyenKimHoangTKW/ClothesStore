using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClotheSstore.App_Start;
using ClotheSstore.Models;

namespace ClotheSstore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class UserAdminController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/UserAdmin
        public ActionResult Index()
        {
            return View(db.UserAdmins.ToList());
        }

        // GET: Admin/UserAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAdmin userAdmin = db.UserAdmins.Find(id);
            if (userAdmin == null)
            {
                return HttpNotFound();
            }
            return View(userAdmin);
        }

        // GET: Admin/UserAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/UserAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserAdmin userAdmin, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                userAdmin.idAdmin = nextId;
                userAdmin.codeAdmin = "ADMIN" + nextId;
                var password = GetMD5(f["PassWord"]);
                if(db.UserAdmins.SingleOrDefault(a => a.userName == userAdmin.userName) != null)
                {
                    ViewBag.ThongBao = "Tài khoản này đã tồn tại, vui lòng chọn tài khoản khác";
                }
                else
                {
                    userAdmin.passWord = password;
                    db.UserAdmins.Add(userAdmin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(userAdmin);
        }
        private int GetNextId()
        {
            int? maxId = db.UserAdmins.Max(t => (int?)t.idAdmin);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        // GET: Admin/UserAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAdmin userAdmin = db.UserAdmins.Find(id);
            if (userAdmin == null)
            {
                return HttpNotFound();
            }
            return View(userAdmin);
        }

        // POST: Admin/UserAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idAdmin,codeAdmin,nameAdmin,phone,userName,passWord,access")] UserAdmin userAdmin)
        {
            if (ModelState.IsValid)
            {
                var password = GetMD5(userAdmin.passWord);
                userAdmin.passWord = password;
                db.Entry(userAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userAdmin);
        }

        // GET: Admin/UserAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAdmin userAdmin = db.UserAdmins.Find(id);
            if (userAdmin == null)
            {
                return HttpNotFound();
            }
            return View(userAdmin);
        }

        // POST: Admin/UserAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAdmin userAdmin = db.UserAdmins.Find(id);
            db.UserAdmins.Remove(userAdmin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
