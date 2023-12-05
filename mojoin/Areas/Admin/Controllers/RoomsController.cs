using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Models;
using mojoin.ViewModel;
using System.Data;
using System.Security.Claims;
using XAct.Library.Settings;
using XAct.Users;
using System.Web;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting.Internal;


namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles = "Staff,Admin", Policy = "StaffOnly")]*/
    public class RoomsController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }
        public RoomsController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        
        
        // GET: Admin/Rooms
        /*public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.Rooms.Include(r => r.RoomType);
            return View(await dbmojoinContext.ToListAsync());

        }*/

		public async Task<IActionResult> Index(int isActive = -1)
		{
			HttpContext.Session.SetInt32("RoomsParams", isActive);
			ViewBag.RoomsParams = isActive;
			var dbmojoinContext = GetListRoomByActive(isActive);
			ViewData["QuyenAcc"] = new SelectList(_context.Roles, "RoleName", "RoleName");
			List<SelectListItem> lsTrangThai = new List<SelectListItem>();
			List<SelectListItem> lsLoaiPhong = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Đã duyệt", Value = "1" });
			lsTrangThai.Add(new SelectListItem() { Text = "Không được duyệt", Value = "2" });
            lsTrangThai.Add(new SelectListItem() { Text = "Bài đăng đang chờ xử lý", Value = "0" });
			
			lsLoaiPhong.Add(new SelectListItem() { Text = "Nhà trọ", Value = "1" });
			lsLoaiPhong.Add(new SelectListItem() { Text = "Căn hộ", Value = "2" });
			lsLoaiPhong.Add(new SelectListItem() { Text = "Ở chung chủ", Value = "3" });
			lsLoaiPhong.Add(new SelectListItem() { Text = "Ở ghép", Value = "4" });
			ViewData["lsLoaiPhong"] = lsLoaiPhong;
			ViewData["lsTrangThai"] = lsTrangThai;
			//
			return View(await dbmojoinContext.ToListAsync());
		}

		public IQueryable<Room> GetListRoomByActive(int isActive)
		{
			ViewBag.RoomsParams = isActive;
			return isActive == -1 ? _context.Rooms.Include(r => r.RoomType) :
				_context.Rooms.Include(r => r.RoomType).Where(x => x.IsActive == isActive);
		}

		public IActionResult ListRoom(int isActive = -1)
		{
			var dbmojoinContext = GetListRoomByActive(isActive);
			return PartialView(dbmojoinContext.ToListAsync());
		}

		// GET: Admin/Rooms/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Rooms == null)
			{
				return NotFound();
			}

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            var roomImages = await _context.RoomImages
            .Where(ri => ri.RoomId == id)
            .ToListAsync();
            ViewBag.RoomImages = roomImages;
            var us = room.UserId;
            var userName = await _context.Users.FirstOrDefaultAsync(u => u.UserId == us);
            ViewData["UserName"] = new SelectList(_context.Users, "UserId", "FirstName");
            
            ViewBag.RoomsParams = HttpContext.Session.GetInt32("RoomsParams");
            return View(room);
        }

		

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

			var room = await _context.Rooms.FindAsync(id);
			if (room == null)
			{
				return NotFound();
			}
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
			return View(room);
		}

		// POST: Admin/Rooms/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("RoomId,UserId,RoomTypeId,Title,Description,Price,Area,NumRooms,NumBathrooms,CreateDate,LastUpdate,IsActive,StreetNumber,Street,Ward,District,City,HasRefrigerator,HasAirConditioner,HasWasher,HasElevator,HasParking,ViewCount")] Room room)
		{
			if (id != room.RoomId)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					int result = await _context.Rooms.Where(x => x.RoomId == room.RoomId).ExecuteUpdateAsync(x =>
					x.SetProperty(p => p.RoomTypeId, room.RoomTypeId)
					.SetProperty(p => p.Title, room.Title)
					.SetProperty(p => p.Description, room.Description)
					.SetProperty(p => p.Price, room.Price)
					.SetProperty(p => p.Area, room.Area)
					.SetProperty(p => p.NumRooms, room.NumRooms)
					.SetProperty(p => p.NumBathrooms, room.NumBathrooms)
					.SetProperty(p => p.LastUpdate, DateTime.Now)
					.SetProperty(p => p.IsActive, room.IsActive)
					.SetProperty(p => p.StreetNumber, room.StreetNumber)
					.SetProperty(p => p.Street, room.Street)
					.SetProperty(p => p.Ward, room.Ward)
					.SetProperty(p => p.District, room.District)
					.SetProperty(p => p.City, room.City)
					.SetProperty(p => p.HasRefrigerator, room.HasRefrigerator)
					.SetProperty(p => p.HasAirConditioner, room.HasAirConditioner)
					.SetProperty(p => p.HasWasher, room.HasWasher)
					.SetProperty(p => p.HasElevator, room.HasElevator)
					.SetProperty(p => p.HasParking, room.HasParking)
					.SetProperty(p => p.ViewCount, room.ViewCount));
					if (result > 0)
					{
						_notyfService.Success("Cập nhật phòng thành công!");
					}
					else
					{
						_notyfService.Success("Không tìm thấy room tương ứng!");
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RoomExists(room.RoomId))
					{
						_notyfService.Success("Có lỗi xảy ra!");
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index), new { isActive = HttpContext.Session.GetInt32("RoomsParams") });
			}

            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            return View(room);
        }
        // GET: Admin/Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.RoomsParams = (int)HttpContext.Session.GetInt32("RoomsParams");

            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, IFormCollection collection)
        {
            int isActive = (int)HttpContext.Session.GetInt32("RoomsParams");

            try
            {
                var findRoom = _context.Rooms
                    .Include(r => r.RoomImages)
                    .FirstOrDefault(x => x.RoomId == id);
                if (findRoom != null)
                {
                    _context.RoomImages.RemoveRange(findRoom.RoomImages);
                    _context.Rooms.Remove(findRoom);
                    _context.SaveChanges();

                    _notyfService.Success("Xóa thành công!");
                }
                else
                {
                    _notyfService.Error("Xóa thất bại!");
                }

                var dbmojoinContext = GetListRoomByActive(isActive);

                return RedirectToAction(nameof(Index), new { isActive });
            }
            catch
            {
                _notyfService.Error("Xóa thất bại!");
            }
            return RedirectToAction(nameof(Index), new { isActive });
        }
        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }



		public IActionResult UpdateRoomActive(int roomId)
		{
			int isActive = (int)HttpContext.Session.GetInt32("RoomsParams");

			try
			{
				var findRoom = _context.Rooms.Where(x => x.RoomId == roomId).FirstOrDefault();
				if (findRoom != null)
				{
                    findRoom.IsActive = 1;
					_context.SaveChanges();

					_notyfService.Success("Cập nhật thành công!");
				}
				else
				{
					_notyfService.Success("Cập nhật thất bại!");
				}

				var dbmojoinContext = GetListRoomByActive(isActive);

				return PartialView("ListRoom", dbmojoinContext);
			}
			catch
			{
				_notyfService.Success("Cập nhật thất bại!");
			}
			return RedirectToAction(nameof(Index), new { isActive });
		}
        [HttpPost]
        public ActionResult UploadMultipleImage(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);

                    //string path = Path.Combine(Server.MapPath("~/images/"), _FileName);
                    //if (System.IO.File.Exists(path))
                    //{
                    //    //nếu hình ảnh đã tồn tại, thì xóa ảnh cũ, cập nhật lại ảnh mới
                    //    System.IO.File.Delete(path);
                    //    file.SaveAs(path);
                    //}
                    //else
                    //{
                    //    file.SaveAs(path);
                    //}
                }
            }
            return RedirectToAction("Index");
        }
		public static bool SendActiceMail(string toMail, string registerUrl)
		{
			try
			{
				using (MailMessage mail = new MailMessage())
				{
					mail.From = new MailAddress("trucnganhuynh001@gmail.com");
					mail.To.Add(toMail);
					mail.Subject = "Đăng bài thành công!";
					mail.Body = GetWebPageContent(registerUrl, true);
					mail.IsBodyHtml= true;

					using (SmtpClient smtp = new SmtpClient("smtp.gmail.com",587)) 
					{
						smtp.UseDefaultCredentials = false;
						smtp.Credentials = new NetworkCredential("trucnganhuynh001@gmail.com", "oosj iioc clci kotz");
						smtp.EnableSsl = true;
						smtp.Send(mail);

					}
				}
				return true;
			}catch(SmtpException ex)
			{

			}catch(Exception ex)
			{

			}
			return false;
		}
		private static string GetWebPageContent(string registerUrl, bool isActice = true)
		{
			string bodyMsg = string.Empty;
			if (isActice)
			{
				StreamReader objStreamReader = new StreamReader("D:\\DACN\\source\\mojoin\\Content\\templates\\ActiceEmailTemplate.html");
				bodyMsg = objStreamReader.ReadToEnd();
				bodyMsg = bodyMsg.Replace("##registerUrl##", registerUrl);
			}
			else
			{
				StreamReader objStreamReader = new StreamReader("D:\\DACN\\source\\mojoin\\Content\\templates\\ResetEmailTemplate.html");
				bodyMsg = objStreamReader.ReadToEnd();
				bodyMsg = bodyMsg.Replace("##newPassword##", registerUrl);
			}

			return bodyMsg;
		}

	}

}
