using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace ClotheSstore.Controllers
{
    public class HomeController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AoSoMiPartial()
        {
            var listProduct = db.Products.Where(p => p.stock == true && p.ProductCategory.codeProductCategory == "DMSP2223PT").Take(6);
            return PartialView(listProduct.ToList());
        }
        public ActionResult BoSuuTapPartial()
        {
            var listProduct = db.ProductCategories.ToList();
            return PartialView(listProduct);
        }
        public ActionResult QuanTayPartial()
        {
            var listProduct = db.Products.Where(p => p.stock == true && p.ProductCategory.codeProductCategory == "DMSP2423PT").Take(6);
            return PartialView(listProduct.ToList());
        }

        public ActionResult ListSanPhamTheoCat(int? id, int? page)
        {
            int pageSize = 15   ;
            int pageNumber = (page ?? 1);
            var listProduct = db.Products.Where(p => p.idProductCategory == id && p.stock == true).OrderBy(p => p.idProduct);
            return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var listProduct = from b in db.Products
                              where b.idProduct == id
                              select b;
    
            return View(listProduct.Single());
        }

        public ActionResult SizeTheoSanPham(int? id)
        {
            var listProduct = db.Product_Size.Where(s => s.idProduct == id);
            return PartialView(listProduct);
        }
    }
}