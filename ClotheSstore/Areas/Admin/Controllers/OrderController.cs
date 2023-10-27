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
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClotheSstore.App_Start;

namespace ClotheSstore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class OrderController : Controller
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderChuaXuLy()
        {
            var lstorder = from d in db.Orders
                           where d.deliveryStatus == "Chờ xác nhận"
                           select d;
            return PartialView(lstorder);
        }
        public ActionResult OrderDaXuLy()
        {
            var lstorder = from d in db.Orders
                           where d.deliveryStatus == "Đang chuẩn bị giao hàng"
                           select d;
            return PartialView(lstorder);
        }
        public ActionResult OrderDangGiao()
        {
            var lstorder = from d in db.Orders
                           where d.deliveryStatus == "Đang giao hàng"
                           select d;
            return PartialView(lstorder);
        }
        public ActionResult OrderGiaoThanhCong()
        {
            var lstorder = from d in db.Orders
                           where d.deliveryStatus == "Giao hàng thành công"
                           select d;
            return PartialView(lstorder);
        }
        public ActionResult OrderDetailNon(int? id)
        {
            var lstorderbyid = db.OrderDetails.Where(o => o.idOrder == id).ToList();
            return View(lstorderbyid);
        }
        public async Task<ActionResult> OrderDetailEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }


            order.deliveryStatus = "Đang chuẩn bị giao hàng";

            db.Entry(order).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ActionResult OrderDetailDaXuLy(int? id)
        {
            var lstorderbyid = db.OrderDetails.Where(o => o.idOrder == id).ToList();
            return View(lstorderbyid);
        }
        public async Task<ActionResult> OrderDetailEditDaXuLy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }


            order.deliveryStatus = "Đang giao hàng";

            db.Entry(order).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ActionResult OrderDetailDangGiaoHang(int? id)
        {
            var lstorderbyid = db.OrderDetails.Where(o => o.idOrder == id).ToList();
            return View(lstorderbyid);
        }
        public async Task<ActionResult> OrderDetailEditDangGiaoHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }


            order.deliveryStatus = "Giao hàng thành công";
            order.deliveryDate = DateTime.Now;
            db.Entry(order).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public ActionResult OrderDetailGiaoHangThanhCong(int? id)
        {
            var lstorderbyid = db.OrderDetails.Where(o => o.idOrder == id).ToList();
            return View(lstorderbyid);
        }

        public ActionResult OrderChuaThanhToan()
        {
            var lstorderbyid = db.Orders.Where(o => o.checkPay == false).ToList();
            return View(lstorderbyid);
        }

        public async Task<ActionResult> OrderXuLyThanhToan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            order.checkPay = true;
            db.Entry(order).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return View();
        }
    }
}