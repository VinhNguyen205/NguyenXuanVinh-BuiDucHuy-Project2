using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;
using Project2.Models;

namespace NguyenXuanVinh_Project2.Controllers
{
    public class GiftsController : Controller
    {
        private readonly Project2DbContext _context;

        public GiftsController(Project2DbContext context)
        {
            _context = context;
        }

        // GET: Gifts
        public async Task<IActionResult> Index()
        {
            // Lấy các sản phẩm là quà tặng (ví dụ: category = "Gift" hoặc có flag IsGift)
            var gifts = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.Category != null && b.Category.CategoryName.ToLower() == "gift")
                .ToListAsync();

            return View(gifts);
        }

        // GET: Gifts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var gift = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (gift == null) return NotFound();

            return View(gift);
        }
    }
}