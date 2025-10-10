using Microsoft.AspNetCore.Mvc;
using NguyenXuanVinh_Project2.Models;
using NguyenXuanVinh_Project2.Helpers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace NguyenXuanVinh_Project2.Controllers
{
    public class CartController : Controller
    {
        private readonly Project2DbContext _context;

        public CartController(Project2DbContext context)
        {
            _context = context;
        }

        // Helper method để cập nhật Session
        private void UpdateCartSession(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Quantity));

            // Tính tổng tiền
            decimal total = 0;
            foreach (var item in cart)
            {
                if (!string.IsNullOrEmpty(item.BookId) && item.Book != null)
                {
                    total += (decimal)(item.Book.Price ?? 0) * item.Quantity;
                }
                else if (item.GiftId.HasValue && item.Gift != null)
                {
                    total += (decimal)(item.Gift.Price ?? 0) * item.Quantity;
                }
            }

            HttpContext.Session.SetString("CartTotal", total.ToString("N0"));
        }

        // Trang giỏ hàng
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Nạp lại Book và Gift từ DB (tránh null)
            foreach (var item in cart)
            {
                if (!string.IsNullOrEmpty(item.BookId))
                {
                    item.Book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);
                }
                else if (item.GiftId.HasValue)
                {
                    item.Gift = _context.Gifts.FirstOrDefault(g => g.GiftId == item.GiftId.Value);
                }
            }

            // Cập nhật lại session với dữ liệu mới
            UpdateCartSession(cart);

            return View(cart);
        }

        // Thêm sách vào giỏ
        [HttpPost]
        public IActionResult AddToCart(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(c => !string.IsNullOrEmpty(c.BookId) && c.BookId == id);
            if (item != null)
            {
                item.Quantity++;
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

            UpdateCartSession(cart);

            return Json(new
            {
                count = cart.Sum(c => c.Quantity),
                total = HttpContext.Session.GetString("CartTotal")
            });
        }

        // Thêm quà vào giỏ
        [HttpPost]
        public IActionResult AddGiftToCart(int id)
        {
            if (id <= 0) return BadRequest();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var gift = _context.Gifts.FirstOrDefault(g => g.GiftId == id);

            if (gift == null) return NotFound();

            var item = cart.FirstOrDefault(c => c.GiftId.HasValue && c.GiftId.Value == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    GiftId = id,
                    Gift = gift,
                    Quantity = 1
                });
            }

            UpdateCartSession(cart);

            TempData["Success"] = $"Đã thêm '{gift.GiftName}' vào giỏ hàng!";
            return RedirectToAction("Index", "Gifts");
        }


        // Cập nhật số lượng sách
        [HttpPost]
        public IActionResult UpdateQuantityBook(string bookId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => !string.IsNullOrEmpty(c.BookId) && c.BookId == bookId);

            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                // Nạp lại Book để tính toán
                item.Book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            }

            UpdateCartSession(cart);

            TempData["Message"] = "Cập nhật giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng quà
        [HttpPost]
        public IActionResult UpdateQuantityGift(int giftId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.GiftId.HasValue && c.GiftId.Value == giftId);

            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                // Nạp lại Gift để tính toán
                item.Gift = _context.Gifts.FirstOrDefault(g => g.GiftId == giftId);
            }

            UpdateCartSession(cart);

            TempData["Message"] = "Cập nhật giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // Xóa sách
        [HttpPost]
        public IActionResult RemoveBook(string bookId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => !string.IsNullOrEmpty(c.BookId) && c.BookId == bookId);
            if (item != null) cart.Remove(item);

            UpdateCartSession(cart);

            TempData["Message"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            return RedirectToAction("Index");
        }

        // Xóa quà
        [HttpPost]
        public IActionResult RemoveGift(int giftId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.GiftId.HasValue && c.GiftId.Value == giftId);
            if (item != null) cart.Remove(item);

            UpdateCartSession(cart);

            TempData["Message"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.SetInt32("CartCount", 0);
            HttpContext.Session.SetString("CartTotal", "0");

            TempData["Message"] = "Đã xóa toàn bộ giỏ hàng!";
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

            HttpContext.Session.Remove("Cart");
            HttpContext.Session.SetInt32("CartCount", 0);
            HttpContext.Session.SetString("CartTotal", "0");

            TempData["Message"] = "Thanh toán thành công!";
            return RedirectToAction("Index");
        }
    }
}