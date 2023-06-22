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
using AspNetCoreHero.ToastNotification.Abstractions;
using XAct.Users;
using X.PagedList;
using mojoin.Extension;
using System.Drawing.Printing;

namespace mojoin.Controllers
{
    public class RoomsController : Controller
    {
        private readonly DbmojoinContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }
        public RoomsController(DbmojoinContext context, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Rooms
        public ActionResult Index(int? page)
        {
            int pageSize = 13; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại (nếu không có, mặc định là 1)

            var dbmojoinContext = db.Rooms
                .Include(r => r.RoomRatings)
                .Include(r => r.RoomReports)
                .Include(r => r.RoomFavorites)
                .Include(r => r.RoomImages)
                .Include(r => r.RoomType)
                .ToPagedList(pageNumber, pageSize);

            return View(dbmojoinContext);
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
            ViewBag.Avt = room.User.Avatar;
            ViewBag.IsYeuThich = isYeuThich;
            ViewBag.SDT = room.User.Phone;
            ViewBag.NgayThamGia = room.User.CreateDate;
            ViewBag.Ho = room.User.LastName;
            ViewBag.Ten = room.User.FirstName;
            ViewBag.Mail = room.User.Email;
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
        public ActionResult SelectedSearch(int? page, string categoryId, string cityId, string districtId, string streetId, string priceId, string areaId)
        {
            IPagedList<Room> searchResults = PerformSearch(page, categoryId, cityId, districtId, streetId, priceId, areaId);
            return PartialView("_ListSearchRoomPartial", searchResults);
        }

        // Hàm để thực hiện tìm kiếm
        protected IPagedList<Room> PerformSearch(int? page,string categoryId, string cityId, string districtId, string streetId, string priceId, string areaId)
        {
            int pageSize = 5; // Số lượng phần tử trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại (nếu không có, mặc định là 1)
            List<Room> searchResults = new List<Room>();

            // Lấy danh sách toàn bộ phòng từ CSDL hoặc từ một nguồn dữ liệu khác
            var allRooms = db.Rooms.ToList().ToPagedList(pageNumber, pageSize); // Hàm này cần được thay thế bằng phương thức truy vấn dữ liệu thực tế

            // Duyệt qua từng phòng và áp dụng điều kiện tìm kiếm
            foreach (var room in allRooms)
            {
                if (room.IsActive != 1)
                {
                    continue; // Bỏ qua phòng không khớp điều kiện
                }

                // So sánh categoryId với RoomTypeId
                if (!string.IsNullOrEmpty(categoryId) && categoryId != "0" && room.RoomTypeId != int.Parse(categoryId))
                    continue; // Bỏ qua phòng không khớp điều kiện

                // Kiểm tra các điều kiện lọc và chỉ áp dụng nếu có giá trị
                if (!string.IsNullOrEmpty(cityId) && room.City != cityId)
                    continue; // Bỏ qua phòng không khớp điều kiện

                if (!string.IsNullOrEmpty(districtId) && room.District != districtId)
                    continue; // Bỏ qua phòng không khớp điều kiện

                if (!string.IsNullOrEmpty(streetId) && room.Street != streetId)
                    continue; // Bỏ qua phòng không khớp điều kiện

                if (!string.IsNullOrEmpty(priceId))
                {
                    int priceValue = int.Parse(priceId);
                    if (priceValue == 1 && room.Price >= 1000000)
                        continue; // Bỏ qua phòng không khớp điều kiện

                    if (priceValue == 2 && (room.Price < 1000000 || room.Price >= 2000000))
                        continue; // Bỏ qua phòng không khớp điều kiện

                    if (priceValue == 3 && (room.Price < 2000000 || room.Price >= 3000000))
                        continue; // Bỏ qua phòng không khớp điều kiện

                    // Các trường hợp khác tương tự
                }

                if (!string.IsNullOrEmpty(areaId))
                {
                    int areaValue = int.Parse(areaId);

                    if (areaValue == 1 && room.Area >= 20)
                        continue; // Bỏ qua phòng không khớp điều kiện

                    if (areaValue == 2 && (room.Area < 20 || room.Area >= 30))
                        continue; // Bỏ qua phòng không khớp điều kiện

                    if (areaValue == 3 && (room.Area < 30 || room.Area >= 50))
                        continue; // Bỏ qua phòng không khớp điều kiện

                    // Các trường hợp khác tương tự
                }

                // Nếu phòng vượt qua tất cả các điều kiện, thì thêm vào danh sách kết quả tìm kiếm
                searchResults.Add(room);
            }

            return searchResults.ToPagedList(pageNumber, pageSize);
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        // Action để xử lý việc gửi tin nhắn
        [HttpPost]
        public ActionResult SendMessage(FormCollection form)
        {
            // Lấy giá trị từ các trường form
            string userMail = form["usermail"];
            string fullName = form["fullName"];
            string email = form["email"];
            string phoneNumber = form["phoneNumber"];
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber) )
            {
                // Hiển thị thông báo lỗi hoặc xử lý lỗi khác tùy theo yêu cầu của bạn
                ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
                return View();
            }
            string emailBody = $"Chào bạn, " +
                $"/n Tôi là: {fullName}, có nhu cầu liên hệ với bạn về bài đăng cho thuê phòng/căn hộ của bạn trên 'mojoin'.  "+
                $"/n Đây là thông tin liên hệ của mình email: {email} hoặc liên hệ: {phoneNumber}."+
                $"/n Mong nhận được phản hồi sớm từ bạn, /n {fullName}.";

/*            _emailService.SendEmail(userMail, "Đặt lại mật khẩu", emailBody);
*/            // Sau khi gửi thành công, bạn có thể chuyển hướng hoặc hiển thị thông báo thành công cho người dùng
            _notyfService.Success("Gửi thành công! Người dùng sẽ liên hệ đến bạn sau!");
            return RedirectToAction("SendMessage");
        }
        [HttpPost]
        public IActionResult SendReport(string roomId, string userId, List<string> errorContents)
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID == null || taikhoanID.ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }

            int parsedRoomId;
            int parsedUserId;

            if (!int.TryParse(roomId, out parsedRoomId) || !int.TryParse(userId, out parsedUserId))
            {
                // Trả về thông báo lỗi nếu roomId hoặc userId không hợp lệ
                return BadRequest("RoomId hoặc UserId không hợp lệ");
            }
            if (errorContents.Count == 0)
            {
                _notyfService.Error("Lỗi báo cáo!");

                // Trả về thông báo nếu danh sách rỗng
                return RedirectToAction("Details", "Rooms", new { id = parsedRoomId });
            }
            // Thực hiện xử lý lưu dữ liệu vào bảng RoomReport
            foreach (var errorContent in errorContents)
            {
                // Tạo một đối tượng RoomReport và lưu các giá trị
                var roomReport = new RoomReport
                {
                    RoomId = parsedRoomId,
                    UserId = parsedUserId,
                    ReportContent = errorContent,
                    IsResolved = 0,
                    CreateDate = DateTime.Now,
                };

                db.RoomReports.Add(roomReport);
                db.SaveChanges();

                _notyfService.Success("Đã gửi báo cáo!");
                return RedirectToAction("Details", "Rooms", new { id = parsedRoomId });
            }

            // Trả về kết quả thành công (hoặc thông báo thành công) cho người dùng
            return Ok("Dữ liệu đã được gửi thành công");
        }




    }
}
