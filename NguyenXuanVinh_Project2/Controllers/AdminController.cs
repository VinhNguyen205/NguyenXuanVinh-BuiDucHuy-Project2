using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;   // cần cho Session
using System;

namespace NguyenXuanVinh_Project2.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Kiểm tra quyền Admin trước khi chạy bất kỳ action nào.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy thông tin từ session
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");
            var username = context.HttpContext.Session.GetString("Username");

            // Nếu chưa đăng nhập hoặc không phải admin
            if (string.IsNullOrEmpty(isAdmin) ||
                !string.Equals(isAdmin, "true", StringComparison.OrdinalIgnoreCase))
            {
                // Chuyển hướng về trang Login của Account
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return; // dừng thực thi action
            }

            // Truyền Username để hiển thị ở View (nếu cần)
            ViewBag.Username = string.IsNullOrEmpty(username) ? "Admin" : username;

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Trang quản trị chính – chỉ Admin mới truy cập được.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Có thể thêm các action quản lý khác cho Admin tại đây
        /// </summary>
        public IActionResult ManageUsers()
        {
            // Ví dụ: hiển thị danh sách user
            return View();
        }
    }
}
