using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecipeProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RecipeProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeProduct>>> GetRecipesProducts()
        {
          if (_context.RecipesProducts == null)
          {
              return NotFound();
          }
            return await _context.RecipesProducts.ToListAsync();
        }

        // GET: api/RecipeProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeProduct>> GetRecipeProduct(Guid id)
        {
          if (_context.RecipesProducts == null)
          {
              return NotFound();
          }
            var recipeProduct = await _context.RecipesProducts.FindAsync(id);

            if (recipeProduct == null)
            {
                return NotFound();
            }

            return recipeProduct;
        }

        // PUT: api/RecipeProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeProduct(Guid id, RecipeProduct recipeProduct)
        {
            if (id != recipeProduct.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipeProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RecipeProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipeProduct>> PostRecipeProduct(RecipeProduct recipeProduct)
        {
          if (_context.RecipesProducts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.RecipesProducts'  is null.");
          }
            _context.RecipesProducts.Add(recipeProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RecipeProductExists(recipeProduct.RecipeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRecipeProduct", new { id = recipeProduct.RecipeId }, recipeProduct);
        }

        // DELETE: api/RecipeProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeProduct(Guid id)
        {
            if (_context.RecipesProducts == null)
            {
                return NotFound();
            }
            var recipeProduct = await _context.RecipesProducts.FindAsync(id);
            if (recipeProduct == null)
            {
                return NotFound();
            }

            _context.RecipesProducts.Remove(recipeProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeProductExists(Guid id)
        {
            return (_context.RecipesProducts?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
