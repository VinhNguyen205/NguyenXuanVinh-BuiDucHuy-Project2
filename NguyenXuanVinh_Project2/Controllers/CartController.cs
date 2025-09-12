using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenXuanVinh_Project2.Models;
using Project2.Models;
using NguyenXuanVinh_Project2.Helpers;
using System.Collections.Generic;
using System.Linq;

public class CartController : Controller
{
    private readonly Project2DbContext _context;

    public CartController(Project2DbContext context)
    {
        _context = context;
    }

    // Trang giỏ hàng
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        // Nạp lại Book từ DB (tránh trường hợp null)
        foreach (var item in cart)
        {
            item.Book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId)!;
        }

        return View(cart);
    }

    // Thêm vào giỏ
    [HttpPost]
    public IActionResult AddToCart(string id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookId == id);
        if (item != null)
        {
            item.Quantity++; // tăng số lượng nếu đã có
        }
        else
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null) return NotFound();

            cart.Add(new CartItem
            {
                BookId = id,
                Book = book,
                Quantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Quantity));

        return Json(new { count = cart.Sum(c => c.Quantity) });
    }

    // Cập nhật số lượng
    [HttpPost]
    public IActionResult UpdateQuantity(string id, int quantity)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookId == id);
        if (item != null && quantity > 0)
        {
            item.Quantity = quantity;
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Quantity));

        return RedirectToAction("Index");
    }

    // Xóa 1 sản phẩm
    [HttpPost]
    public IActionResult Remove(string id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookId == id);
        if (item != null)
        {
            cart.Remove(item);
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Quantity));

        return RedirectToAction("Index");
    }

    // Thanh toán (demo)
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        if (!cart.Any())
        {
            TempData["Message"] = "Giỏ hàng trống!";
            return RedirectToAction("Index");
        }

        // 👉 Ở đây bạn có thể lưu Order vào DB
        HttpContext.Session.Remove("Cart");
        HttpContext.Session.SetInt32("CartCount", 0);

        TempData["Message"] = "Thanh toán thành công!";
        return RedirectToAction("Index");
    }
}
