using ClotheSstore.App_Start;
using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClotheSstore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var ordernon = "Chờ xác nhận";
            IList<Product> listsp = db.Products.ToList();
            IList<Customer> listcustomer = db.Customers.ToList();
            decimal SumPrice = db.OrderDetails.Sum(o => o.price);
            IList<Order> lstordertransit = db.Orders.Where(p => p.deliveryStatus.Equals(ordernon)).ToList();
            ViewBag.TotalProduct = listsp.Count;
            ViewBag.TotalCustomer = listcustomer.Count;
            ViewBag.SumPrice = SumPrice;
            ViewBag.lstordertransit = lstordertransit.Count;
            return View();
        }
    }
}