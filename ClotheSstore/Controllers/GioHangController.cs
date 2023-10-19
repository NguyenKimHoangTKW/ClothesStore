using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClotheSstore.Controllers
{
    public class GioHangController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        // GET: Cart
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url, FormCollection f)
        {
            List<GioHang> lstCart = LayGioHang();
            string selectedSize = f["txtSize"].ToString(); 

            GioHang sp = lstCart.Find(n => n.iSanPham == ms && n.iSize == selectedSize);

            if (sp == null)
            {
                sp = new GioHang(ms);
                sp.iSize = selectedSize;
                lstCart.Add(sp);
            }
            else
            {
                sp.iSoLuong += int.Parse(f["txtSoLuong"].ToString());
            }
            return Redirect(url);
        }


        private int TongSoLuong()
        {
            int iSumQuantity = 0;
            List<GioHang> lstCart = Session["GioHang"] as List<GioHang>;
            if (lstCart != null)
            {
                iSumQuantity = lstCart.Sum(n => n.iSoLuong);
            }
            return iSumQuantity;
        }

        private double TongTien()
        {
            double dSumPrice = 0;
            List<GioHang> lstCart = Session["GioHang"] as List<GioHang>;
            if (lstCart != null)
            {
                dSumPrice = lstCart.Sum(n => n.ThanhTien);
            }
            return dSumPrice;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstCart = LayGioHang();
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstCart);
        }
        public ActionResult GioHangPartial()
        {

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSanPham(int iMaSanPham)
        {
            List<GioHang> lstCart = LayGioHang();
            GioHang sp = lstCart.SingleOrDefault(n => n.iSanPham == iMaSanPham);
            if (sp != null)
            {
                lstCart.RemoveAll(n => n.iSanPham == iMaSanPham);
                if (lstCart.Count == 0)
                {
                    return RedirectToAction("Index", "Home");

                }

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaSanPham,string iSize, FormCollection f)
        {
            List<GioHang> lstCart = LayGioHang();
            GioHang sp = lstCart.FirstOrDefault(n => n.iSanPham == iMaSanPham && n.iSize == iSize);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaToanBoSanPham()
        {
            List<GioHang> lstCart = LayGioHang();
            lstCart.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Order()
        {
            if (Session["Customer"] == null || Session["Customer"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstCart = LayGioHang ();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstCart);
        }

        [HttpPost]
        public ActionResult Order(FormCollection f)
        {
            Order order = new Order();
            Customer customer = (Customer)Session["Customer"];
            List<GioHang> lstCart = LayGioHang();
            order.codeOrder = "HĐ" + DateTime.Now.ToString("yyyyMMddHHmmss");
            order.idCustomer = customer.idCustomer;
            order.orderDate = DateTime.Now;
            TimeSpan threeDays = new TimeSpan(7, 0, 0, 0);
            order.deliveryDate = order.orderDate + threeDays;
            order.checkPay = false;
            order.deliveryStatus = "Đã tiếp nhận đơn hàng";
            db.Orders.Add(order);
            db.SaveChanges();
            foreach (var item in lstCart)
            {
                OrderDetail orderdetail = new OrderDetail();
                orderdetail.idOrder = order.idOrder;
                orderdetail.idProduct = item.iSanPham;
                orderdetail.quantity = item.iSoLuong;
                orderdetail.Size = item.iSize;
                orderdetail.price = (decimal)item.dDonGia;
                db.OrderDetails.Add(orderdetail);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("OrderConfirm", "GioHang");

        }

        public ActionResult OrderConfirm()
        {
            return View();
        }
    }
}