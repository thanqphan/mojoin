using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Authorization;

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
        public ActionResult Search(string keyword)
        {

            var all = db.Rooms.Include(r => r.RoomImages).Where(x => x.Description.Contains(keyword));

            return View(all);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SelectedSearch(string categoryId, string cityId, string districtId, string streetId, string priceId, string areaId)
        {
            List<Room> searchResults = PerformSearch(categoryId, cityId, districtId, streetId, priceId, areaId);
            return PartialView("_ListSearchRoomPartial", searchResults);
        }

        // Hàm để render partial view thành chuỗi HTML
        /*protected string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }*/

        // Hàm để thực hiện tìm kiếm
        protected List<Room> PerformSearch(string categoryId, string cityId, string districtId, string streetId, string priceId, string areaId)
        {
            List<Room> searchResults = new List<Room>();

            // Lấy danh sách toàn bộ phòng từ CSDL hoặc từ một nguồn dữ liệu khác
            List<Room> allRooms = db.Rooms.ToList(); // Hàm này cần được thay thế bằng phương thức truy vấn dữ liệu thực tế
            
            // Duyệt qua từng phòng và áp dụng điều kiện tìm kiếm
            foreach (var room in allRooms)
            {
                if (room.IsActive == 0 && room.IsActive==2)
                {
                    continue;
                }
                // So sánh categoryId với RoomTypeId
                if (categoryId != "0" && room.RoomTypeId != int.Parse(categoryId))
                    continue; // Bỏ qua phòng không khớp điều kiện

                // So sánh districtId với District
                if (districtId != "" && room.District != districtId)
                    continue; // Bỏ qua phòng không khớp điều kiện

                // So sánh streetId với Street
                if (streetId != "" && room.Street != streetId)
                    continue; // Bỏ qua phòng không khớp điều kiện

                // So sánh priceId với Price
                int priceValue = int.Parse(priceId);
                switch (priceValue)
                {
                    case 0: // Thỏa thuận
                            // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 1: // Dưới 1 triệu
                        if (room.Price >= 1000000)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 2: // 1 triệu - 2 triệu
                        if (room.Price < 1000000 || room.Price >= 2000000)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 3: // 2 triệu - 3 triệu
                        if (room.Price < 2000000 || room.Price >= 3000000)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    // Các case khác tương tự

                    default: // Trường hợp không xác định
                             // Bỏ qua phòng không khớp điều kiện
                        break;
                }

                // So sánh areaId với Area
                int areaValue = int.Parse(areaId);
                switch (areaValue)
                {
                    case 0: // Chưa xác định
                            // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 1: // Dưới 20 m2
                        if (room.Area >= 20)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 2: // 20 - 30 m2
                        if (room.Area < 20 || room.Area >= 30)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    case 3: // 30 - 50 m2
                        if (room.Area < 30 || room.Area >= 50)
                            continue; // Bỏ qua phòng không khớp điều kiện
                        break;
                    // Các case khác tương tự

                    default: // Trường hợp không xác định
                             // Bỏ qua phòng không khớp điều kiện
                        break;
                }

                // Nếu phòng vượt qua tất cả các điều kiện, thì thêm vào danh sách kết quả tìm kiếm
                searchResults.Add(room);
            }

            return searchResults;
        }

    }
}
