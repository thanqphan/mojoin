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
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(DbmojoinContext context, INotyfService notyfService)
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
        public IActionResult Inactive()
        {
            return View();
        }
        [Route("my-Account.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.UserId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {

                    return View(khachhang);
                }

            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register.html", Name = "DangKy")]
        public async Task<IActionResult> Register(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //*						string salt = Utilities.GetRandomKey();
                    User user = new User
                    {
                        RolesId = 3,
                        LastName = taikhoan.LastName.Trim(),
                        FirstName = taikhoan.FirstName.Trim(),
                        Email = taikhoan.Email.Trim(),
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Password = taikhoan.Password,
                        //*+ salt.Trim()).ToMD5()*//*,
                        IsActive = true,
                        //*Salt = salt,
                        CreateDate = DateTime.Now
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
                            new Claim("FirstName", user.FirstName),
                            new Claim("LastName",user.LastName),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("Number",user.Phone.ToString()),
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công!");
                        return RedirectToAction("Index", "MyAccount");
                    }
                    catch
                    {
                        return RedirectToAction("Register", "Account");
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
        [Route("login.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "MyAccount");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel taikhoan/*, string returnUrl*/)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isPhone = Utilities.IsInteger(taikhoan.UserName);
                    /*                    bool isPhone = Utilities.IsInteger(taikhoan.UserName);
                    */
                    if (!isPhone) return View(taikhoan);
                    /*if (!isPhone) return View(taikhoan);*/
                    var user = _context.Users.AsNoTracking().SingleOrDefault(x => x.Phone.Trim() == taikhoan.UserName /*|| x.Phone.Trim()==taikhoan.UserName*/);
                    if (user == null)
                    {
                        _notyfService.Error("Vui lòng tạo tài khoản!");
                        return RedirectToAction("Register");
                    }


                    string pass = taikhoan.Password /*+ khachhang.Salt.Trim()).ToMD5()*/;
                    if (user.Password != pass)
                    {
                        _notyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(taikhoan);
                    }
                    //kiem tra xem account co bi disable hay khong

                    if (user.IsActive == false)
                    {
                        return View("Inactive", "Account");
                    }
                    if (user.RolesId == 1 || user.RolesId == 2)
                    {
                        //Luu Session MaKh
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("UserId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim("FirstName", user.FirstName),
                            new Claim("LastName",user.LastName),
                            new Claim("RoleAcc",user.RolesId.ToString()),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("UserPhone",user.Phone.ToString()),
                            new Claim("UserAvt",user.Avatar.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng nhập thành công");

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (user.RolesId == 3)
                    {
                        //Luu Session MaKh
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("UserId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim("FirstName", user.FirstName),
                            new Claim("LastName",user.LastName),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("UserPhone",user.Phone.ToString()),
                            new Claim("UserAvatar",user.Avatar.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng nhập thành công");

                        return RedirectToAction("Index", "MyAccount");
                    }

                    /*if (string.IsNullOrEmpty(returnUrl))
					{
						return RedirectToAction("Index", "MyAccount");
					}
					else
					{
						return Redirect(returnUrl);
					}*/
                }
            }
            catch
            {
                _notyfService.Error("Đăng nhập không thành công!");
                return RedirectToAction("Register", "Account");
            }
            _notyfService.Error("Đăng nhập không thành công!");
            return View(taikhoan);
        }
        [HttpGet]
        [Route("logout.html", Name = "DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Rooms");
        }

       
    }
}
