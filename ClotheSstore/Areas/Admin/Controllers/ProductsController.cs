using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
    public class ProductsController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/Products
        public ActionResult Index(int product = 0)
        {
           
            var products = db.Products.Include(p => p.ProductCategory);
            if (product != 0)   
                products = products.Where(c => c.idProductCategory == product);
            ViewBag.product = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory");
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase thumb, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                product.idProduct = nextId;
                product.codeProduct = "SP" + nextId.ToString("2023PT");
                if (db.Products.SingleOrDefault(p => p.nameProduct == product.nameProduct) != null)
                {
                    ViewBag.ThongBao = "Sản phẩm này đã tồn tại, Vui lòng nhập sản phẩm khác";
                }
                else if (thumb != null && thumb.ContentLength > 0)
                {
                    string _Head = Path.GetFileNameWithoutExtension(thumb.FileName);
                    string _Tail = Path.GetExtension(thumb.FileName);
                    string fullLink = _Head + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + _Tail;
                    string _path = Path.Combine(Server.MapPath("~/Areas/Admin/Images/Product_Images"), fullLink);
                    thumb.SaveAs(_path);
                    product.thumb = fullLink;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn một tệp ảnh.");
                }

            }
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory", product.idProductCategory);
            return View(product);
        }
        private int GetNextId()
        {
            // Tìm giá trị id tiếp theo trong bảng
            int? maxId = db.Products.Max(t => (int?)t.idProduct);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory", product.idProductCategory);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "idProduct,codeProduct,nameProduct,describe,thumb,stock,price,idProductCategory,updateDay")] Product product, HttpPostedFileBase Thumb, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Thumb != null)
                    {
                        string _Head = Path.GetFileNameWithoutExtension(Thumb.FileName);
                        string _Tail = Path.GetExtension(Thumb.FileName);
                        string fullLink = _Head + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + _Tail;
                        string _path = Path.Combine(Server.MapPath("~/Areas/Admin/Images/Product_Images"), fullLink);
                        Thumb.SaveAs(_path);
                        product.thumb = fullLink;
                        _path = Path.Combine(Server.MapPath("~/Areas/Admin/Images/Product_Images"), form["oldimage"]);

                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);

                    }
                    else
                    product.thumb = form["oldimage"];
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }
                return RedirectToAction("Index");
            }
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory", product.idProductCategory);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, Product product)
        {
            product = db.Products.Find(id);
            if (db.Product_Size.SingleOrDefault(pz => pz.idProduct == product.idProduct) != null)
            {
                ViewBag.ThongBao = "Không thể xóa vì Danh sách Size theo sản phẩm đang tồn tại sản phẩm này, vui lòng kiểm tra lại dữ liệu !!!";
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);     
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
