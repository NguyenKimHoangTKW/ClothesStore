using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}