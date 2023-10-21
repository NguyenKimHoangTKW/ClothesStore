using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClotheSstore.Models;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;
namespace ClotheSstore.Areas.Admin.Controllers
{
    public class ProductSizeController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/ProductSize
        public ActionResult Index( int productSizeid = 0)
        {
            

            var product_Size = db.Product_Size.Include(p => p.Product).Include(p => p.Size);

            if (productSizeid != 0)
                product_Size = product_Size.Where(c => c.idProduct == productSizeid);

            ViewBag.productSizeid = new SelectList(db.Products, "idProduct", "nameProduct");

            return View(product_Size.ToList());
        }

        // GET: Admin/ProductSize/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            return View(product_Size);
        }

        // GET: Admin/ProductSize/Create
        public ActionResult Create()
        {
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct");
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize");
            return View();
        }
       
        // POST: Admin/ProductSize/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                product_Size.idProduct_Size = nextId;
                db.Product_Size.Add(product_Size);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", product_Size.idProduct);
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize", product_Size.idSize);
            return View(product_Size);
        }
        private int GetNextId()
        {
            // Tìm giá trị id tiếp theo trong bảng
            int? maxId = db.Product_Size.Max(t => (int?)t.idProduct_Size);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        // GET: Admin/ProductSize/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", product_Size.idProduct);
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize", product_Size.idSize);
            return View(product_Size);
        }

        // POST: Admin/ProductSize/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduct_Size,idProduct,idSize,quantity")] Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Size).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", product_Size.idProduct);
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize", product_Size.idSize);
            return View(product_Size);
        }

        // GET: Admin/ProductSize/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            return View(product_Size);
        }

        // POST: Admin/ProductSize/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Size product_Size = db.Product_Size.Find(id);
            db.Product_Size.Remove(product_Size);
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
    }
}
