using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using System;
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

        // ===================== LOGIN =====================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập → quay về trang chủ
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _context.Accounts
                               .AsNoTracking()
                               .FirstOrDefault(a => a.Username == username
                                                 && a.Password == password // ⚠️ Nên hash thực tế
                                                 && (a.Active ?? false));
            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu, hoặc tài khoản bị khóa!";
                return View();
            }

            // 👉 Lưu session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("AccountId", user.AccountId);
            HttpContext.Session.SetString("Role", user.Role ?? "Member");

            // ⚡ Quan trọng: set IsAdmin cho AdminController
            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetString("IsAdmin", "true");
            }
            else
            {
                HttpContext.Session.Remove("IsAdmin");
            }

            // Nếu có returnUrl hợp lệ → redirect về đó
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // Điều hướng theo Role
            return string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "Home");
        }

        // ===================== REGISTER MEMBER =====================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

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
                Password = model.Password, // ⚠️ Nên hash
                Active = true,
                Role = "Member"
            };

            _context.Accounts.Add(newUser);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }

        // ===================== REGISTER ADMIN =====================
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAdmin(Account model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login");

            if (!ModelState.IsValid) return View(model);

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

        // ===================== LOGOUT =====================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
