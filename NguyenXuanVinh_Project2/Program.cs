using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// ================== 1. Add services ==================
var connectionString = builder.Configuration.GetConnectionString("Project2");

// Đăng ký DbContext
builder.Services.AddDbContext<Project2DbContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// 👉 Thêm Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Bắt buộc cho GDPR
});

// 👉 Nếu sau này dùng [Authorize] thì cần Identity/Cookie
// builder.Services.AddAuthentication(...);
// builder.Services.AddAuthorization(...);

var app = builder.Build();

// ================== 2. Configure middleware pipeline ==================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 👉 Bật Session **TRƯỚC** Authorization
app.UseSession();

// 👉 Nếu có Authentication thì gọi ở đây
// app.UseAuthentication();

app.UseAuthorization();

// Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
