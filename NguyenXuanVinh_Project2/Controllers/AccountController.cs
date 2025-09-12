using Microsoft.AspNetCore.Mvc;
using NguyenXuanVinh_Project2.Models;
using Project2.Models;
using NguyenXuanVinh_Project2.Helpers;
namespace NguyenXuanVinh_Project2.Controllers
{
    public class AccountController : Controller
    {
        private readonly Project2DbContext _context;

        public AccountController(Project2DbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _context.Accounts
                .FirstOrDefault(a => a.Username == username && a.Password == password && a.Active == true);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu, hoặc tài khoản bị khóa!";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("AccountId", user.AccountId);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin == true ? "true" : "false");

            // Điều hướng
            if (user.IsAdmin == true)
            {
                return RedirectToAction("Index", "Admin"); // Trang quản lý
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Trang khách
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
