using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly IEmailService _emailService;

        public AccountController(DbmojoinContext context, INotyfService notyfService, IEmailService emailService)
        {
            _context = context;
            _notyfService = notyfService;
            _emailService = emailService;
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
                        CreateDate = DateTime.Now,
                        Balance=0
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
        [HttpGet]
        [AllowAnonymous]
        [Route("forgotpass.html", Name = "QuenMatKhau")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotpass.html", Name = "QuenMatKhau")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

                if (user != null)
                {
                    // Tạo token đặt lại mật khẩu
                    string resetToken = Guid.NewGuid().ToString();

                    // Lưu token vào cơ sở dữ liệu hoặc bảng riêng để xác thực việc đặt lại mật khẩu
                    user.ResetPasswordToken = resetToken;
                    _context.SaveChanges();

                    // Gửi email chứa liên kết đặt lại mật khẩu
                    string resetUrl = Url.Action("ResetPassword", "Account", new { token = resetToken }, Request.Scheme);
                    string emailBody = $@"
                        <html>
                        <head>
                            <title>[Motel to Join] Đặt lại mật khẩu</title>
                        </head>
                        <body>
                            <div style='font-family: 'Arial', sans-serif; max-width: 600px; margin: 0 auto;'>
                                <h2>[Motel to Join] Đặt lại mật khẩu</h2>
                                <p>Chào bạn {user.FirstName} {user.LastName},</p>
                                <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu từ bạn. Để hoàn tất quá trình, vui lòng nhấp vào liên kết dưới đây:</p>
                                <a href='{resetUrl}' style='display: inline-block; padding: 10px 20px; background-color: #3bacb6; color: #fff; text-decoration: none; border-radius: 5px;'>Liên kết đặt lại mật khẩu</a>
                                <p>Nếu bạn không thực hiện yêu cầu này, bạn có thể bỏ qua email này. Liên kết sẽ hết hạn trong một khoảng thời gian ngắn.</p>
                                <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                                <p>Trân trọng,<br>Đội ngũ hỗ trợ Motel to Join</p>
                            </div>
                        </body>
                        </html>
                    ";

                    // Gửi email
                    _emailService.SendEmail(user.Email, "[Motel to Join] Đặt lại mật khẩu", emailBody);

                    _notyfService.Success("Email đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra hộp thư đến của bạn.");

                    return RedirectToAction("Login");
                }

                _notyfService.Error("Email không tồn tại trong hệ thống.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetPasswordToken == token);

            if (user == null)
            {
                _notyfService.Error("Liên kết đặt lại mật khẩu không hợp lệ.");
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var token = model.Token;
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    if (ModelState.TryGetValue(key, out ModelStateEntry entry) && entry.Errors.Any())
                    {
                        // Trường có tên là "key" có lỗi validation
                        var errorMessage = entry.Errors[0].ErrorMessage;
                        // Xử lý lỗi ở đây
                    }
                    else
                    {
                        // Trường có tên là "key" không có lỗi validation
                    }
                }

                _notyfService.Error("Đổi mật khẩu không thành công!");
                return View();
            }
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.ResetPasswordToken == model.Token);

                if (user == null)
                {
                    _notyfService.Error("Liên kết đặt lại mật khẩu không hợp lệ.");
                    return RedirectToAction("Login");
                }

                // Cập nhật mật khẩu mới cho người dùng
                user.Password = model.Password;
                user.ResetPasswordToken = null;
                _context.SaveChanges();

                _notyfService.Success("Mật khẩu đã được đặt lại thành công. Vui lòng đăng nhập bằng mật khẩu mới.");

                return RedirectToAction("Login");
            }

            return View(model);
        }



    }
}
