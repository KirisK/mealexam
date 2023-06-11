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
    public class RecipeProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipeProduct
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecipesProducts.Include(r => r.Product).Include(r => r.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipeProduct/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.RecipesProducts == null)
            {
                return NotFound();
            }

            var recipeProduct = await _context.RecipesProducts
                .Include(r => r.Product)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeProduct == null)
            {
                return NotFound();
            }

            return View(recipeProduct);
        }

        // GET: RecipeProduct/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description");
            return View();
        }

        // POST: RecipeProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequiredAmount,ProductId,RecipeId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RecipeProduct recipeProduct)
        {
            if (ModelState.IsValid)
            {
                recipeProduct.RecipeId = Guid.NewGuid();
                _context.Add(recipeProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", recipeProduct.ProductId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description", recipeProduct.RecipeId);
            return View(recipeProduct);
        }

        // GET: RecipeProduct/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RecipesProducts == null)
            {
                return NotFound();
            }

            var recipeProduct = await _context.RecipesProducts.FindAsync(id);
            if (recipeProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", recipeProduct.ProductId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description", recipeProduct.RecipeId);
            return View(recipeProduct);
        }

        // POST: RecipeProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequiredAmount,ProductId,RecipeId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RecipeProduct recipeProduct)
        {
            if (id != recipeProduct.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeProductExists(recipeProduct.RecipeId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName", recipeProduct.ProductId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description", recipeProduct.RecipeId);
            return View(recipeProduct);
        }

        // GET: RecipeProduct/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.RecipesProducts == null)
            {
                return NotFound();
            }

            var recipeProduct = await _context.RecipesProducts
                .Include(r => r.Product)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeProduct == null)
            {
                return NotFound();
            }

            return View(recipeProduct);
        }

        // POST: RecipeProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.RecipesProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RecipesProducts'  is null.");
            }
            var recipeProduct = await _context.RecipesProducts.FindAsync(id);
            if (recipeProduct != null)
            {
                _context.RecipesProducts.Remove(recipeProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeProductExists(Guid id)
        {
          return (_context.RecipesProducts?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
