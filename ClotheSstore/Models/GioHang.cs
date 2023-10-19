using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClotheSstore.Models
{
    public class GioHang
    {
        private dbClothesStoreEntities db = new dbClothesStoreEntities();
        public int iSanPham { get; set; }
        public string sTenSanPham { get; set; }
        public string sAnhSanPham { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public string iSize { get; set; }
        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int ms)
        {
            iSanPham = ms;
            Product_Size b = db.Product_Size.First(n => n.idProduct == iSanPham);
            sTenSanPham = b.Product.nameProduct;
            sAnhSanPham = b.Product.thumb;
            dDonGia = double.Parse(b.Product.price.ToString());
            iSize = b.Size.nameSize;
            iSoLuong = 1;
        }
    }
}