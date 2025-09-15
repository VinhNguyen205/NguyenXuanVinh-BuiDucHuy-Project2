using Microsoft.AspNetCore.Mvc;
using NguyenXuanVinh_Project2.Models;
using Project2.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        public IActionResult Login(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                // Nếu đã login thì chuyển hướng luôn
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _context.Accounts
                .FirstOrDefault(a => a.Username == username
                                  && a.Password == password
                                  && (a.Active ?? false));

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu, hoặc tài khoản bị khóa!";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("AccountId", user.AccountId);
            HttpContext.Session.SetString("Role", user.Role ?? "Member");

            // Nếu có returnUrl hợp lệ → redirect về đó
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Mặc định điều hướng theo Role
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Accounts.Any(a => a.Username == model.Username))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
                return View(model);
            }

            var newUser = new Account
            {
                AccountId = Guid.NewGuid().ToString(),
                Username = model.Username,
                Email = model.Email,
                Password = model.Password, // ⚠️ TODO: hash password
                Active = true,
                Role = "Member"
            };

            _context.Accounts.Add(newUser);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }

        // GET: /Account/RegisterAdmin
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");

            return View();
        }

        // POST: /Account/RegisterAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAdmin(Account model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            if (_context.Accounts.Any(a => a.Username == model.Username))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
                return View(model);
            }

            model.AccountId = Guid.NewGuid().ToString();
            model.Role = "Admin";
            model.Active = true;

            _context.Accounts.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Tạo Admin mới thành công!";
            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
