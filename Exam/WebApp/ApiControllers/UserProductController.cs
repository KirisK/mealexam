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
    public class UserProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProduct>>> GetUserProducts()
        {
          if (_context.UserProducts == null)
          {
              return NotFound();
          }
            return await _context.UserProducts.ToListAsync();
        }

        // GET: api/UserProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProduct>> GetUserProduct(Guid id)
        {
          if (_context.UserProducts == null)
          {
              return NotFound();
          }
            var userProduct = await _context.UserProducts.FindAsync(id);

            if (userProduct == null)
            {
                return NotFound();
            }

            return userProduct;
        }

        // PUT: api/UserProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProduct(Guid id, UserProduct userProduct)
        {
            if (id != userProduct.AppUserId)
            {
                return BadRequest();
            }

            _context.Entry(userProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProductExists(id))
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

        // POST: api/UserProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserProduct>> PostUserProduct(UserProduct userProduct)
        {
          if (_context.UserProducts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserProducts'  is null.");
          }
            _context.UserProducts.Add(userProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserProductExists(userProduct.AppUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserProduct", new { id = userProduct.AppUserId }, userProduct);
        }

        // DELETE: api/UserProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProduct(Guid id)
        {
            if (_context.UserProducts == null)
            {
                return NotFound();
            }
            var userProduct = await _context.UserProducts.FindAsync(id);
            if (userProduct == null)
            {
                return NotFound();
            }

            _context.UserProducts.Remove(userProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProductExists(Guid id)
        {
            return (_context.UserProducts?.Any(e => e.AppUserId == id)).GetValueOrDefault();
        }
    }
}
