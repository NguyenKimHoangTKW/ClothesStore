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
using System.Linq.Dynamic; // nhúng vào tập tin 
using System.Linq.Expressions;
namespace ClotheSstore.Areas.Admin.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();

        // GET: Admin/ProductCategories
        public ActionResult Index(int? size, int? page, string sortProperty, string searchString, string sortOrder = "", int productid = 0)
        {
            ViewBag.Keyword = searchString;
            ViewBag.Subject = productid;
            var products = db.ProductCategories.Include(p => p.Products);

            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(b => b.nameProductCategory.Contains(searchString));


            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(b => b.nameProductCategory.Contains(searchString));

               


            if (sortOrder == "asc") ViewBag.SortOrder = "desc";
            if (sortOrder == "desc") ViewBag.SortOrder = "";
            if (sortOrder == "") ViewBag.SortOrder = "asc";
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "nameProductCategory";

            // Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc")
                products = products.OrderBy(sortProperty + " desc");
            else
                products = products.OrderBy(sortProperty);

            //   phân trang
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

            //  Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.Size = items;
            ViewBag.CurrentSize = size;
            // Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            //  Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            int checkTotal = (int)(products.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            //  Trả kết quả về Views
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "idProductCategory,codeProductCategory,nameProductCategory")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (db.ProductCategories.SingleOrDefault(pc => pc.nameProductCategory == productCategory.nameProductCategory) != null)
                {
                    ViewBag.ThongBao = "Danh mục sản phẩm này đã tồn tại, vui lòng nhập tên danh mục khác";
                }
                else
                {
                    int nextId = GetNextId();
                    productCategory.idProductCategory = nextId;
                    productCategory.codeProductCategory = "DMSP" + nextId.ToString("2023PT"); ;
                    db.ProductCategories.Add(productCategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(productCategory);
        }
        private int GetNextId()
        {
            // Tìm giá trị id tiếp theo trong bảng
            int? maxId = db.ProductCategories.Max(t => (int?)t.idProductCategory);

            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            {
                return 1;
            }
        }
        // GET: Admin/ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProductCategory,codeProductCategory,nameProductCategory")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
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
