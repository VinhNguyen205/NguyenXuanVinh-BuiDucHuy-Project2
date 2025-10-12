using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using System;
using System.Threading.Tasks;

namespace NguyenXuanVinh_Project2.Controllers
{
    public class AdminController : Controller
    {
        private readonly Project2DbContext _context;

        public AdminController(Project2DbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            var username = context.HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang này!";
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            ViewBag.Username = string.IsNullOrEmpty(username) ? "Admin" : username;
            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy thống kê từ database
                ViewBag.TotalBooks = await _context.Books.CountAsync();
                ViewBag.TotalCategories = await _context.Categories.CountAsync();
                ViewBag.TotalPublishers = await _context.Publishers.CountAsync();

                // Đếm số Gift items - ĐÃ SỬA LẠI CHO ĐÚNG
                // Giả sử trong DbContext của bạn có DbSet tên là "Gifts"
                ViewBag.TotalGifts = await _context.Gifts.CountAsync();

                ViewBag.TotalOrders = 0;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        public IActionResult ManageUsers()
        {
            return View();
        }

        public async Task<IActionResult> Reports()
        {
            var report = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalRevenue = 0,
                BooksByCategory = await _context.Categories
                    .Include(c => c.Books)
                    .Select(c => new {
                        CategoryName = c.CategoryName,
                        Count = c.Books.Count
                    })
                    .ToListAsync()
            };

            return View(report);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đã đăng xuất thành công!";
            return RedirectToAction("Login", "Account");
        }
    }
}