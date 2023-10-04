using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClotheSstore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            IList<Product> listsp = db.Products.ToList();
            IList<Customer> listcustomer = db.Customers.ToList();
            ViewBag.TotalProduct = listsp.Count;
            ViewBag.TotalCustomer = listcustomer.Count;
            return View();
        }
    }
}