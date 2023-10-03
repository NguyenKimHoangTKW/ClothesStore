﻿using System;
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
        public ActionResult Index(int? size, int? page, string sortProperty, string searchString, string sortOrder = "", int product = 0)
        {
            ViewBag.Keyword = searchString;
            ViewBag.Subject = product;
            var products = db.Products.Include(p => p.ProductCategory).Include(p => p.OrderDetails);

            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(b => b.nameProduct.Contains(searchString));

          
            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(b => b.nameProduct.Contains(searchString));

            if (product != 0)
                products = products.Where(c => c.idProductCategory == product);

            ViewBag.ProductCategories = new SelectList(db.ProductCategories, "idProductCategory", "nameProductCategory"); // danh sách Category         

            if (sortOrder == "asc") ViewBag.SortOrder = "desc";
            if (sortOrder == "desc") ViewBag.SortOrder = "";
            if (sortOrder == "") ViewBag.SortOrder = "asc";
            // 2.1. Tạo thuộc tính sắp xếp mặc định là "Title"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "nameProduct";

            // 2.2. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc")
                products = products.OrderBy(sortProperty + " desc");
            else
                products = products.OrderBy(sortProperty);

            // 3 Đoạn code sau dùng để phân trang
            ViewBag.Page = page;

            // 3.1. Tạo danh sách chọn số trang
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });

            // 3.2. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.Size = items;
            ViewBag.CurrentSize = size;
            // 3.3. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 3.4. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 3.5. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 3.6 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            int checkTotal = (int)(products.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            // 4. Trả kết quả về Views
            return View(products.ToPagedList(pageNumber, pageSize));
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
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "codeProductCategory");
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
            var _nameProduct = collection["TenProduct"];
            var _describe = collection["MoTa"];
            var _quantity = collection["SoLuong"];
            var _price = collection["Gia"];
            if (ModelState.IsValid)
            {
                int nextId = GetNextId();
                product.idProduct = nextId;
                product.codeProduct = "SP" + nextId.ToString("2023PT");
                if (string.IsNullOrEmpty(product.nameProduct))
                {
                    ViewData["err1"] = "Tên sản phẩm không được rỗng";
                }
                else if (string.IsNullOrEmpty(_quantity))
                {
                    ViewData["err2"] = "Số lượng không được rỗng";
                }
                else if (string.IsNullOrEmpty(_price))
                {
                    ViewData["err3"] = "Giá không được rỗng";
                }
                else if (string.IsNullOrEmpty(_describe))
                {
                    ViewData["err4"] = "Nhập 'Không' hoặc mô tả khác không để trống";
                }
                else if (db.Products.SingleOrDefault(p => p.nameProduct == _nameProduct) != null)
                {
                    ViewBag.ThongBao = "Sản phẩm này đã tồn tại, Vui lòng nhập sản phẩm khác";
                }
                else if (thumb != null && thumb.ContentLength > 0)
                {
                    string _Head = Path.GetFileNameWithoutExtension(thumb.FileName);
                    string _Tail = Path.GetExtension(thumb.FileName);
                    string fullLink = _Head + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + _Tail;
                    string _path = Path.Combine(Server.MapPath("~/Areas/Admin/Images/Product_Images"), fullLink);

                    product.quantity = int.Parse( _quantity);
                    product.price = float.Parse (_price);
                    product.describe = _describe;
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
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "codeProductCategory", product.idProductCategory);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduct,codeProduct,nameProduct,describe,thumb,quantity,price,idProductCategory,updateDay")] Product product, HttpPostedFileBase Thumb, FormCollection form)
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
            ViewBag.idProductCategory = new SelectList(db.ProductCategories, "idProductCategory", "codeProductCategory", product.idProductCategory);
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
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
