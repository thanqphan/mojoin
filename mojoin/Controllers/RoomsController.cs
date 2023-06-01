﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Web;


namespace mojoin.Controllers
{
    public class RoomsController : Controller
    {
        
        DbmojoinContext db = new DbmojoinContext();

        // GET: Rooms
        public ActionResult Index()
        {
            var dbmojoinContext = db.Rooms.Include(r => r.RoomRatings).Include(r => r.RoomReports).Include(r => r.RoomFavorites).Include(r => r.RoomImages).Include(r => r.RoomType);
            return View(dbmojoinContext.ToList());
        }
        public ActionResult ShowImage(int id)
        {
            var roomImage =db.RoomImages
                .Where(r => r.RoomId == id)
                .Select(r => r.Image)
                .FirstOrDefault();
            if (roomImage == null)
            {
                return NotFound();
            }

            // Return the image as a file result
            return File(roomImage, "image/jpeg");
        }
        public ActionResult GetRoomImages(int id)
        {
            var roomImages = db.RoomImages
                .Where(ri => ri.RoomId == id)
                .ToList();

            return View(roomImages);
        }
        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            var room = db.Rooms
                 .Include(r => r.User)
                 .Include(r => r.RoomImages)
                 .Where(ri => ri.RoomId == id)
                 .ToList()
                 .FirstOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            // Lấy giá trị của taikhoanID từ session
            var taikhoanID = HttpContext.Session.GetString("UserId");

            // Kiểm tra xem phòng đã được thêm vào danh sách yêu thích của người dùng hay chưa
            bool isYeuThich = false;
            if (taikhoanID != null && taikhoanID.ToString() != "")
            {
                var yeuThich = db.RoomFavorites.SingleOrDefault(n => n.RoomId == id && n.UserId == int.Parse(taikhoanID));
                if (yeuThich != null)
                {
                    isYeuThich = true;
                }
            }
            ViewBag.IsYeuThich = isYeuThich;
            ViewBag.SDT = room.User.Phone;
            ViewBag.NgayThamGia = room.User.CreateDate;
            ViewBag.Ho = room.User.LastName;
            ViewBag.Ten = room.User.FirstName;
            return View(room);
        }
        public ActionResult GetNameUser(int id)
        {
            var room = db.Rooms
                 .Where(r => r.RoomId == id)
/*                 .Select(r => new { r.RoomId, UserFullName = r.User.Fullname })
*/                 .FirstOrDefault();

            if (room == null)
            {
                return NotFound();
            }

/*            ViewBag.UserFullName = room.UserFullName;
*/            return View(room);
        }      
    }
}
