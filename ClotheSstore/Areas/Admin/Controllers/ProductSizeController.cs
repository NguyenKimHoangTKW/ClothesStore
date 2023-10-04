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
        public ActionResult Index(int? size, int? page, string sortProperty, string searchString, string sortOrder = "", int productSize = 0)
        {
            
            ViewBag.Keyword = searchString;
            ViewBag.Subject = productSize;
            var product_Size = db.Product_Size.Include(p => p.Product).Include(p => p.Size);

            if (!String.IsNullOrEmpty(searchString))
                product_Size = product_Size.Where(b => b.Product.nameProduct.Contains(searchString));

            if (productSize != 0)
                product_Size = product_Size.Where(c => c.idProduct == productSize);

            ViewBag.productSize = new SelectList(db.Products, "idProduct", "nameProduct");

            if (sortOrder == "asc") ViewBag.SortOrder = "desc";
            if (sortOrder == "desc") ViewBag.SortOrder = "";
            if (sortOrder == "") ViewBag.SortOrder = "asc";
            // 2.1. Tạo thuộc tính sắp xếp mặc định là "Title"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "idProduct";

            // 2.2. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc")
                product_Size = product_Size.OrderBy(sortProperty + " desc");
            else
                product_Size = product_Size.OrderBy(sortProperty);

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
            int checkTotal = (int)(product_Size.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            // 4. Trả kết quả về Views
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
        public ActionResult Edit([Bind(Include = "idProduct_Size,idProduct,idSize,quantity,quantityinstock")] Product_Size product_Size)
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
