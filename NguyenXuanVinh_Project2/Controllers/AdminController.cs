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

        /// <summary>
        /// Kiểm tra quyền Admin trước khi chạy bất kỳ action nào.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy thông tin từ session
            var role = context.HttpContext.Session.GetString("Role");
            var username = context.HttpContext.Session.GetString("Username");

            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                // Lưu thông báo lỗi vào TempData
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang này!";
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Truyền Username để hiển thị ở View
            ViewBag.Username = string.IsNullOrEmpty(username) ? "Admin" : username;
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Trang Dashboard quản trị với thống kê
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy thống kê từ database
                ViewBag.TotalBooks = await _context.Books.CountAsync();
                ViewBag.TotalCategories = await _context.Categories.CountAsync();
                ViewBag.TotalPublishers = await _context.Publishers.CountAsync();

                // Đếm số Gift items
                ViewBag.TotalGifts = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Category != null &&
                               (b.Category.CategoryName.ToLower() == "gift" ||
                                b.Category.CategoryName.ToLower() == "quà tặng"))
                    .CountAsync();

                // Nếu có bảng Orders thì uncomment dòng này
                // ViewBag.TotalOrders = await _context.Orders.CountAsync();
                ViewBag.TotalOrders = 0;

                // Top 5 sách bán chạy (nếu có cột SoldCount)
                // ViewBag.TopBooks = await _context.Books
                //     .OrderByDescending(b => b.SoldCount)
                //     .Take(5)
                //     .ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                TempData["ErrorMessage"] = "Có lỗi khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Quản lý người dùng (nếu cần)
        /// </summary>
        public IActionResult ManageUsers()
        {
            // Nếu có bảng Users
            // var users = await _context.Users.ToListAsync();
            // return View(users);

            return View();
        }

        /// <summary>
        /// Xem báo cáo thống kê chi tiết
        /// </summary>
        public async Task<IActionResult> Reports()
        {
            var report = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalRevenue = 0, // Tính từ Orders nếu có
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

        /// <summary>
        /// Action đăng xuất admin
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đã đăng xuất thành công!";
            return RedirectToAction("Login", "Account");
        }
    }
}