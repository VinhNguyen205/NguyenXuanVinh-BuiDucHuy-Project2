using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;

namespace NguyenXuanVinh_Project2.Controllers
{
    public class GiftsController : Controller
    {
        private readonly Project2DbContext _context;

        public GiftsController(Project2DbContext context)
        {
            _context = context;
        }

        // ========================
        // READ (INDEX + DETAILS)
        // ========================

        // GET: Gifts
        public async Task<IActionResult> Index()
        {
            var gifts = await _context.Gifts.ToListAsync();
            return View(gifts);
        }

        // GET: Gifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var gift = await _context.Gifts
                .FirstOrDefaultAsync(g => g.GiftId == id);

            if (gift == null)
                return NotFound();

            return View(gift);
        }

        // ========================
        // CREATE
        // ========================

        // GET: Gifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gifts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GiftName,Price,Description,Picture,Stock")] Gift gift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gift);
        }

        // ========================
        // EDIT
        // ========================

        // GET: Gifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return NotFound();

            return View(gift);
        }

        // POST: Gifts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GiftId,GiftName,Price,Description,Picture,Stock")] Gift gift)
        {
            if (id != gift.GiftId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Gifts.Any(e => e.GiftId == gift.GiftId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gift);
        }

        // ========================
        // DELETE
        // ========================

        // GET: Gifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var gift = await _context.Gifts
                .FirstOrDefaultAsync(g => g.GiftId == id);
            if (gift == null)
                return NotFound();

            return View(gift);
        }

        // POST: Gifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
