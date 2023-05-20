using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using XAct;
using XAct.Domain.Repositories;
using System.Security.Claims;
using XAct.Users;

namespace mojoin.Controllers
{
    public class YeuThichController : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        //public List<Yeuthich> Layyeuthich()
        //{
        //    var yt = HttpContext.Session.GetString("Yeuthich");
        //    List<Yeuthich> lstGiohang = JsonConvert.DeserializeObject<List<Yeuthich>>(yt);

        //    if (lstGiohang == null)
        //    {
        //        lstGiohang = new List<Yeuthich>();
        //        //HttpContext.Session.GetString("Yeuthich") = lstGiohang;
        //    }
        //    return lstGiohang;
        //}
        public List<Yeuthich> Layyeuthich()
        {
            string json = HttpContext.Session.GetString("Yeuthich");
            List<Yeuthich> lstYeuthich;
            if (json == null)
            {
                lstYeuthich = new List<Yeuthich>();
                json = JsonConvert.SerializeObject(lstYeuthich);
                HttpContext.Session.SetString("Yeuthich", json);
            }
            else
            {
                lstYeuthich = JsonConvert.DeserializeObject<List<Yeuthich>>(json);
            }
            return lstYeuthich;
        }
        public ActionResult ThemYeuThich(int id, string strURL)
        {
            List<Yeuthich> lstYeuthich = Layyeuthich();
            Yeuthich yt = lstYeuthich.Find(n => n.RoomId == id);
            if (yt == null)
            {
                yt = new Yeuthich(id);
                lstYeuthich.Add(yt);
                return Redirect(strURL);
            }
            else
            {
                yt.Soluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuongYeuThich()
        {
            int tsl = 0;
            string lstYeuThich = HttpContext.Session.GetString("YeuThich");
            List<Yeuthich> lstYeuthich;
            if (lstYeuThich != null)
            {
                tsl = lstYeuThich.Count();
                HttpContext.Session.SetString("YeuThich", lstYeuThich);
            }
            else
            {
                lstYeuthich = JsonConvert.DeserializeObject<List<Yeuthich>>(lstYeuThich);
            }
            return tsl;
        }
        public ActionResult YeuThich(int id)
        {
            var roomIDs = db.RoomFavorites.Include(r => r.Room).Include(r => r.User)
            .Where(rf => rf.UserId == id)
            .ToList();
            ViewBag.tongsoluong = roomIDs.Count();
            return View(roomIDs);
        }
        public ActionResult UserPartial(int id)
        {
            var roomIDs = db.Users.Include(r => r.Rooms)
            .Where(rf => rf.UserId == id)
            .ToList();

            return View(roomIDs);
        }
        [HttpGet]
        public ActionResult LuuYeuThich()
        {

            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "DanhSachSanPham");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);

        }
        //public ActionResult YeuThich(int id)
        //{
        //    // Lấy UserId từ thông tin người dùng hiện tại
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    // Kiểm tra nếu UserId không null hoặc trống
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        // Tiếp tục xử lý với userId
        //        var roomIDs = db.RoomFavorites.Include(r => r.Room).Include(r => r.User)
        //            .Where(rf => rf.UserId == int.Parse(userId))
        //            .ToList();

        //        return View(roomIDs);
        //    }

        //    // Xử lý khi không có UserId
        //    // Ví dụ: Redirect hoặc trả về một trạng thái không hợp lệ
        //    return RedirectToAction("Error");
        //}


        //public ActionResult YeuThich()
        //{
        //    List<Yeuthich> lstGiohang = Layyeuthich();
        //    return View(lstGiohang);
        //}

        public ActionResult YeuThichPartial()
        {           
            return View();
        }
    }
}
