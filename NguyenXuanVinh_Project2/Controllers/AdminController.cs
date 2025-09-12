using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NguyenXuanVinh_Project2.Helpers;


namespace NguyenXuanVinh_Project2.Controllers
{
    public class AdminController : Controller
    {
        // Chặn truy cập nếu không phải Admin
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");

            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "true")
            {
                // Chuyển về trang login
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return; // ngăn chạy tiếp
            }

            base.OnActionExecuting(context);
        }

        // Trang chính cho Admin
        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
    }
}
