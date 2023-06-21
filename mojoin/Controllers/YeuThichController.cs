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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using mojoin.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace mojoin.Controllers
{
    [Authorize]
    public class YeuThichController : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        public INotyfService _notyfService { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public YeuThichController( INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }
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
        [Route("yeu-thich.html", Name = "CaNhan")]
        public IActionResult YeuThich()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            // Truy vấn danh sách yêu thích từ CSDL
            var roomFavorites = db.RoomFavorites.Include(r => r.Room).Include(r => r.User)
                .Where(rf => rf.UserId == int.Parse(userId))
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
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (taikhoanID == null || taikhoanID.ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin phòng theo id
            Room s = db.Rooms.Single(n => n.RoomId == id);

            // Tạo mới đối tượng RoomFavorite và lưu thông tin
            RoomFavorite ctdh = new RoomFavorite();
            ctdh.RoomId = s.RoomId;
            ctdh.UserId = int.Parse(taikhoanID);
            db.RoomFavorites.Add(ctdh);
            db.SaveChanges();
            _notyfService.Success("Đã yêu thích!");
            return RedirectToAction("Details", "Rooms", new { id = id });
        }
        public ActionResult XoaYeuThich(int id)
        {
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (taikhoanID == null || taikhoanID.ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }

            // Tìm đối tượng RoomFavorite theo RoomId và UserId
            var ctdh = db.RoomFavorites.SingleOrDefault(n => n.RoomId == id && n.UserId == int.Parse(taikhoanID));

            if (ctdh != null)
            {
                // Nếu tìm thấy đối tượng RoomFavorite thì xóa khỏi database
                db.RoomFavorites.Remove(ctdh);
                db.SaveChanges();
            }

            // Chuyển hướng về trang danh sách yêu thích
            _notyfService.Error("Đã gỡ yêu thích!");
            return RedirectToAction("Details", "Rooms", new { id = id });
        }
        public ActionResult YeuThichPartial()
        {           
            return View();
        }
        
    }
}
