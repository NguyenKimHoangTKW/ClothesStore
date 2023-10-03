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

namespace ClotheSstore.Areas.Admin.Controllers
{
    public class ProductSizeController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/ProductSize
        public ActionResult Index(int? size, int? page)
        {
            var product_Size = db.Product_Size.Include(p => p.Product).Include(p => p.Size).OrderBy(p => p.idProduct);
            ViewBag.Page = page;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.Size = items;
            ViewBag.CurrentSize = size;
            page = page ?? 1;

            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            int checkTotal = (int)(product_Size.ToList().Count / pageSize) + 1;
            if (pageNumber > checkTotal) pageNumber = checkTotal;
            return View(product_Size.ToPagedList(pageNumber, pageSize));
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
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "codeProduct");
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize");
            return View();
        }

        // POST: Admin/ProductSize/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProduct,idSize,quantity,quantityinstock")] Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                db.Product_Size.Add(product_Size);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "codeProduct", product_Size.idProduct);
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize", product_Size.idSize);
            return View(product_Size);
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
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "codeProduct", product_Size.idProduct);
            ViewBag.idSize = new SelectList(db.Sizes, "idSize", "nameSize", product_Size.idSize);
            return View(product_Size);
        }

        // POST: Admin/ProductSize/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduct,idSize,quantity,quantityinstock")] Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Size).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "codeProduct", product_Size.idProduct);
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
