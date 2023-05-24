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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YeuThichController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
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
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            // Truy vấn danh sách yêu thích từ CSDL
            var roomFavorites = db.RoomFavorites.Include(r => r.Room).Include(r => r.User)
                .Where(rf => rf.UserId == int.Parse(taikhoanID))
                .ToList();

            // Chuyển đổi danh sách yêu thích sang ViewModel Yeuthich
            var yeuthichList = roomFavorites.Select(rf => new Yeuthich
            {
                FavoriteId = rf.FavoriteId,
                RoomId = rf.Room.RoomId,
                UserId = rf.UserId,
                Title = rf.Room.Title,
                StreetNumber = rf.Room.StreetNumber,
                Street = rf.Room.Street,
                Ward = rf.Room.Ward,
                District = rf.Room.District,
                City = rf.Room.City,
                NumRooms = rf.Room.NumRooms,
                NumBathrooms = rf.Room.NumBathrooms,
                Area = rf.Room.Area,
                Price = rf.Room.Price,
                Description = rf.Room.Description,
                CreateDate = rf.Room.CreateDate
            }).ToList();
            ViewBag.tongsoluong = roomFavorites.Count();
            // Truyền danh sách yêu thích sang View
            return View(yeuthichList);
        }
        public ActionResult UserPartial(int id)
        {
            var roomIDs = db.Users.Include(r => r.Rooms)
            .Where(rf => rf.UserId == id)
            .ToList();

            return View(roomIDs);
        }
        public ActionResult LuuYeuThich(int id)
        {
            Room s = new Room();
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (taikhoanID == null || taikhoanID.ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            List<Yeuthich> gh = Layyeuthich();
                RoomFavorite ctdh = new RoomFavorite();
                ctdh.RoomId = s.RoomId;
                ctdh.UserId = int.Parse(taikhoanID);
                s = db.Rooms.Single(n => n.RoomId == s.RoomId);               
                db.RoomFavorites.Add(ctdh);
                db.SaveChanges();
                return View();
        }
        //public ActionResult LuuYeuThich(int id)
        //{
        //    var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        //    if (taikhoanID == null || taikhoanID.ToString() == "")
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    Room dh = db.Rooms.Single(r => r.RoomId == id);
        //    RoomFavorite kh = new RoomFavorite();
        //    kh.RoomId = dh.RoomId;
        //    kh.UserId = int.Parse(taikhoanID);
        //    db.RoomFavorites.Add(kh);
        //    db.SaveChanges();
        //    return View(dh);
        //}
        //[HttpPost]
        //public ActionResult LuuYeuThich(FormCollection collection)
        //{

        //}

        public ActionResult YeuThichPartial()
        {           
            return View();
        }
    }
}
