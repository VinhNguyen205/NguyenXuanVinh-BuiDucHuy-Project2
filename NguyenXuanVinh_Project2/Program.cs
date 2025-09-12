using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using Project2.Models;

var builder = WebApplication.CreateBuilder(args);

// Kết nối CSDL
var connectionString = builder.Configuration.GetConnectionString("Project2");
builder.Services.AddDbContext<Project2DbContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// 👉 Thêm Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Cấu hình pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 👉 Bật Session
app.UseSession();

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
