using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain.App;

namespace WebApp.Controllers
{
    public class UserProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserProduct
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserProducts.Include(u => u.AppUser).Include(u => u.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserProduct/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.UserProducts == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProducts
                .Include(u => u.AppUser)
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.AppUserId == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // GET: UserProduct/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
            return View();
        }

        // POST: UserProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvailableAmount,ProductId,AppUserId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] UserProduct userProduct)
        {
            if (ModelState.IsValid)
            {
                userProduct.AppUserId = Guid.NewGuid();
                _context.Add(userProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userProduct.AppUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", userProduct.ProductId);
            return View(userProduct);
        }

        // GET: UserProduct/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.UserProducts == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProducts.FindAsync(id);
            if (userProduct == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userProduct.AppUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", userProduct.ProductId);
            return View(userProduct);
        }

        // POST: UserProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AvailableAmount,ProductId,AppUserId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] UserProduct userProduct)
        {
            if (id != userProduct.AppUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProductExists(userProduct.AppUserId))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userProduct.AppUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", userProduct.ProductId);
            return View(userProduct);
        }

        // GET: UserProduct/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.UserProducts == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProducts
                .Include(u => u.AppUser)
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.AppUserId == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // POST: UserProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.UserProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserProducts'  is null.");
            }
            var userProduct = await _context.UserProducts.FindAsync(id);
            if (userProduct != null)
            {
                _context.UserProducts.Remove(userProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProductExists(Guid id)
        {
          return (_context.UserProducts?.Any(e => e.AppUserId == id)).GetValueOrDefault();
        }
    }
}
