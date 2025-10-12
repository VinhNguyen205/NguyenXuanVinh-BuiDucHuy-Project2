using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using System;
using System.Threading.Tasks; 

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
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel()); // <-- Sử dụng ViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // ⚠️ QUAN TRỌNG: Trong thực tế, bạn PHẢI HASH MẬT KHẨU
                // Code này chỉ dành cho mục đích demo
                var user = await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Username == model.Username && a.Password == model.Password);

                if (user != null && (user.Active ?? false))
                {
                    // Lưu session
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("AccountId", user.AccountId);
                    HttpContext.Session.SetString("Role", user.Role ?? "Member");

                    TempData["SuccessMessage"] = $"Chào mừng {user.Username} đã trở lại!";

                    // Điều hướng
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                        ? RedirectToAction("Index", "Admin")
                        : RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa.";
                }
            }
            // Nếu model không hợp lệ hoặc đăng nhập thất bại, hiển thị lại form
            return View(model);
        }

        // ===================== REGISTER MEMBER =====================
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel()); // <-- Sử dụng ViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Accounts.AnyAsync(a => a.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã được sử dụng.");
                    return View(model);
                }

                if (await _context.Accounts.AnyAsync(a => a.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                var newUser = new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, // ⚠️ Nên hash mật khẩu
                    Active = true,
                    Role = "Member"
                };

                _context.Accounts.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // ===================== REGISTER ADMIN (Cải tiến) =====================
        // [Authorize(Roles = "Admin")] // Cách bảo vệ tốt hơn là dùng Authorize
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            // Kiểm tra quyền Admin qua Session (cách cơ bản)
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Login");
            }
            return View(new RegisterViewModel()); // Tái sử dụng RegisterViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Forbid(); // Trả về lỗi 403 Forbidden
            }

            // Xóa validation của AgreeToTerms vì form admin không có
            ModelState.Remove("AgreeToTerms");

            if (ModelState.IsValid)
            {
                if (await _context.Accounts.AnyAsync(a => a.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                    return View(model);
                }

                var newAdmin = new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, // ⚠️ Nên hash
                    Active = true,
                    Role = "Admin" // <-- Gán quyền Admin
                };

                _context.Accounts.Add(newAdmin);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Tạo tài khoản Admin '{newAdmin.Username}' thành công!";
                return RedirectToAction("Index", "Admin"); // Quay về trang quản trị
            }
            return View(model);
        }

        // ===================== LOGOUT =====================
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }
    }
}