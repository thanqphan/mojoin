using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Helper;
using mojoin.Models;
using mojoin.ViewModel;
using System.Security.Claims;

namespace mojoin.Controllers
{
	public class AccountController : Controller
	{
		[Authorize]
		public class AccountsController : Controller
		{
			/*private readonly DbmojoinContext _context;
			public INotyfService _notyfService { get; }
			public AccountsController(DbmojoinContext context, INotyfService notyfService)
			{
				_context = context;
				_notyfService = notyfService;
			}
			[HttpGet]
			[AllowAnonymous]
			public IActionResult ValidatePhone(string Phone)
			{
				try
				{
					var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
					if (khachhang != null)
						return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

					return Json(data: true);

				}
				catch
				{
					return Json(data: true);
				}
			}
			[HttpGet]
			[AllowAnonymous]
			public IActionResult ValidateEmail(string Email)
			{
				try
				{
					var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
					if (khachhang != null)
						return Json(data: "Email : " + Email + " đã được sử dụng");
					return Json(data: true);
				}
				catch
				{
					return Json(data: true);
				}
			}
			[HttpGet]
			[AllowAnonymous]
			[Route("dang-ky.html", Name = "DangKy")]
			public IActionResult DangkyTaiKhoan()
			{
				return View();
			}

			[HttpPost]
			[AllowAnonymous]
			[Route("dang-ky.html", Name = "DangKy")]
			public async Task<IActionResult> DangkyTaiKhoan(RegisterViewModel taikhoan)
			{
				try
				{
					if (ModelState.IsValid)
					{
*//*						string salt = Utilities.GetRandomKey();
*//*						User user = new User
						{
							RolesId = 3,
							Fullname = taikhoan.FullName,
							Phone = taikhoan.Phone.Trim().ToLower(),
							Password = taikhoan.Password *//*+ salt.Trim()).ToMD5()*//*,
							IsActive = true,
*//*							Salt = salt,
*//*							CreateDate = DateTime.Now
						};
						try
						{
							_context.Add(user);
							await _context.SaveChangesAsync();
							//Lưu Session User
							HttpContext.Session.SetString("UserId", user.UserId.ToString());
							var taikhoanID = HttpContext.Session.GetString("UserId");

							//Identity
							var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name,khachhang.FullName),
							new Claim("CustomerId", khachhang.CustomerId.ToString())
						};
							ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
							ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
							await HttpContext.SignInAsync(claimsPrincipal);
							_notyfService.Success("Đăng ký thành công");
							return RedirectToAction("Dashboard", "Accounts");
						}
						catch
						{
							return RedirectToAction("DangkyTaiKhoan", "Accounts");
						}
					}
					else
					{
						return View(taikhoan);
					}
				}
				catch
				{
					return View(taikhoan);
				}
			}
			[AllowAnonymous]
			[Route("dang-nhap.html", Name = "DangNhap")]
			public IActionResult Login(string returnUrl = null)
			{
				var taikhoanID = HttpContext.Session.GetString("CustomerId");
				if (taikhoanID != null)
				{
					return RedirectToAction("Dashboard", "Accounts");
				}
				return View();
			}
			[HttpPost]
			[AllowAnonymous]
			[Route("dang-nhap.html", Name = "DangNhap")]
			public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl)
			{
				try
				{
					if (ModelState.IsValid)
					{
						bool isEmail = Utilities.IsValidEmail(customer.UserName);
						if (!isEmail) return View(customer);

						var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);

						if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
						string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
						if (khachhang.Password != pass)
						{
							_notyfService.Success("Thông tin đăng nhập chưa chính xác");
							return View(customer);
						}
						//kiem tra xem account co bi disable hay khong

						if (khachhang.Active == false)
						{
							return RedirectToAction("ThongBao", "Accounts");
						}

						//Luu Session MaKh
						HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
						var taikhoanID = HttpContext.Session.GetString("CustomerId");

						//Identity
						var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, khachhang.FullName),
						new Claim("CustomerId", khachhang.CustomerId.ToString())
					};
						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
						ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
						await HttpContext.SignInAsync(claimsPrincipal);
						_notyfService.Success("Đăng nhập thành công");
						if (string.IsNullOrEmpty(returnUrl))
						{
							return RedirectToAction("Dashboard", "Accounts");
						}
						else
						{
							return Redirect(returnUrl);
						}
					}
				}
				catch
				{
					return RedirectToAction("DangkyTaiKhoan", "Accounts");
				}
				return View(customer);
			}
			[HttpGet]
			[Route("dang-xuat.html", Name = "DangXuat")]
			public IActionResult Logout()
			{
				HttpContext.SignOutAsync();
				HttpContext.Session.Remove("CustomerId");
				return RedirectToAction("Index", "Home");
			}

			[HttpPost]
			public IActionResult ChangePassword(ChangePasswordViewModel model)
			{
				try
				{
					var taikhoanID = HttpContext.Session.GetString("CustomerId");
					if (taikhoanID == null)
					{
						return RedirectToAction("Login", "Accounts");
					}
					if (ModelState.IsValid)
					{
						var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
						if (taikhoan == null) return RedirectToAction("Login", "Accounts");
						var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
						{
							string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
							taikhoan.Password = passnew;
							_context.Update(taikhoan);
							_context.SaveChanges();
							_notyfService.Success("Đổi mật khẩu thành công");
							return RedirectToAction("Dashboard", "Accounts");
						}
					}
				}
				catch
				{
					_notyfService.Success("Thay đổi mật khẩu không thành công");
					return RedirectToAction("Dashboard", "Accounts");
				}
				_notyfService.Success("Thay đổi mật khẩu không thành công");
				return RedirectToAction("Dashboard", "Accounts");
			}
		}
	}*/
		}
	}
}