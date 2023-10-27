using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClotheSstore.Models;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using ClotheSstore.App_Start;

namespace ClotheSstore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomersController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index()
        {
            var customer = db.Customers.Include(p => p.Orders);
            return View(customer.ToList());
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
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

        // GET: Admin/Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                customer.idCustomer = nextId;
                customer.codeCustomer = "KH" + nextId.ToString("2023PT");
                customer.credate = DateTime.Now;
                if (string.IsNullOrEmpty(customer.nameCustomer))
                {
                    ViewData["err1"] = "Không được để trống tên";
                }
                else if (string.IsNullOrEmpty(customer.userName))
                {
                    ViewData["err2"] = "Không được để trống tên đăng nhập";
                }
                else if (string.IsNullOrEmpty(customer.passWord))
                {
                    ViewData["err3"] = "Không được để trống mật khẩu";
                }
                else if (string.IsNullOrEmpty(customer.email))
                {
                    ViewData["err4"] = "Không được để trống email";
                }
                else if (string.IsNullOrEmpty(customer.address))
                {
                    ViewData["err5"] = "Không được để trống địa chỉ";
                }
                else if (string.IsNullOrEmpty(customer.phone))
                {
                    ViewData["err6"] = "Không được để trống SDT";
                }
                else if (db.Customers.SingleOrDefault(c => c.userName == customer.userName) != null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                }
                else if (db.Customers.SingleOrDefault(c => c.email == customer.email) != null)
                {
                    ViewBag.ThongBao = "Email đã tồn tại";
                }
                else
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }            
            }

            return View(customer);
        }
        private int GetNextId()
        {
            // Tìm giá trị id tiếp theo trong bảng
            int? maxId = db.Customers.Max(t => (int?)t.idCustomer);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        // GET: Admin/Customers/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCustomer,codeCustomer,nameCustomer,userName,passWord,birthDay,email,address,phone,credate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            if(db.Orders.Any(p => p.idCustomer == customer.idCustomer))
            {
                ViewBag.ThongBao = "Người dùng này hiện đang có trong Hóa đơn, vui lòng xóa hết hóa đơn của người dùng này trước khi xóa";
            }
            else
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
