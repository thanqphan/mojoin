using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Models;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
// Add services to the container.
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddOptions<SendGridSettings>()
    .Bind(builder.Configuration.GetSection("SendGridSettings"))
    .ValidateDataAnnotations();

// Đọc cấu hình từ appsettings.json
builder.Services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(p =>
                {
                    p.Cookie.Name = "UserLoginCookie";
                    p.ExpireTimeSpan = TimeSpan.FromDays(1);
                    p.LoginPath = "/login.html";
                    p.LogoutPath = "/dang-xuat/html";
                    p.AccessDeniedPath = "/not-found.html";
                });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("RoleAcc", "1"));
    options.AddPolicy("AdminOrStaff", policy => policy.RequireAssertion(context =>
        context.User.HasClaim(c =>
            (c.Type == "RoleAcc" && c.Value == "1") || // admin
            (c.Type == "RoleAcc" && c.Value == "2")    // staff
        )
    ));

});
// Register DbContext
builder.Services.AddDbContext<DbmojoinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbRoomFinder")));
// Register HtmlEncoder
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rooms}/{action=Index}/{id?}");

app.Run();
