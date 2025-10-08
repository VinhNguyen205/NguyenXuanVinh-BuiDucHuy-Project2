using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;  

namespace NguyenXuanVinh_Project2.Controllers
{
    public class OrderGiftsController : Controller
    {
        private readonly Project2DbContext _context;

        public OrderGiftsController(Project2DbContext context)
        {
            _context = context;
        }

        // GET: OrderGifts
        public async Task<IActionResult> Index()
        {
            var project2DbContext = _context.OrderGifts   // 👈 đổi OrderGift -> OrderGifts
                .Include(o => o.Gift)
                .Include(o => o.OrderBook);
            return View(await project2DbContext.ToListAsync());
        }

        // GET: OrderGifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderGift = await _context.OrderGifts
                .Include(o => o.Gift)
                .Include(o => o.OrderBook)
                .FirstOrDefaultAsync(m => m.OrderGiftId == id);
            if (orderGift == null)
            {
                return NotFound();
            }

            return View(orderGift);
        }

        // GET: OrderGifts/Create
        public IActionResult Create()
        {
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftName");   // 👈 Gift -> Gifts
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId"); // 👈 OrderBook -> OrderBooks
            return View();
        }

        // POST: OrderGifts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderGiftId,OrderId,GiftId,Quantity,Price,TotalMoney")] OrderGift orderGift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderGift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftName", orderGift.GiftId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderGift.OrderId);
            return View(orderGift);
        }

        // GET: OrderGifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderGift = await _context.OrderGifts.FindAsync(id);
            if (orderGift == null)
            {
                return NotFound();
            }
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftName", orderGift.GiftId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderGift.OrderId);
            return View(orderGift);
        }

        // POST: OrderGifts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderGiftId,OrderId,GiftId,Quantity,Price,TotalMoney")] OrderGift orderGift)
        {
            if (id != orderGift.OrderGiftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderGift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderGiftExists(orderGift.OrderGiftId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftName", orderGift.GiftId);
            ViewData["OrderId"] = new SelectList(_context.OrderBooks, "OrderId", "OrderId", orderGift.OrderId);
            return View(orderGift);
        }

        // GET: OrderGifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderGift = await _context.OrderGifts
                .Include(o => o.Gift)
                .Include(o => o.OrderBook)
                .FirstOrDefaultAsync(m => m.OrderGiftId == id);
            if (orderGift == null)
            {
                return NotFound();
            }

            return View(orderGift);
        }

        // POST: OrderGifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderGift = await _context.OrderGifts.FindAsync(id);
            if (orderGift != null)
            {
                _context.OrderGifts.Remove(orderGift);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderGiftExists(int id)
        {
            return _context.OrderGifts.Any(e => e.OrderGiftId == id);
        }
    }
}
