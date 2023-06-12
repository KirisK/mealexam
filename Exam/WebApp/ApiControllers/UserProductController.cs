using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Public.DTO.v1;
using Public.DTO.v1.Error;
using Public.DTO.v1.Mappers;
using WebApp.Extensions;

namespace WebApp.ApiControllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserProductMapper _mapper;

        public UserProductController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = new UserProductMapper(mapper);
        }

        // GET: api/UserProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProduct>>> GetUserProducts()
        {
            var userProducts = await _context.UserProducts
                .Include(upo => upo.Product)
                .Where(up => up.AppUserId == User.GetUserId())
                .ToListAsync();

            return Ok(userProducts.Select(up => _mapper.Map(up)));
        }

        // GET: api/UserProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProduct>> GetUserProduct(Guid id)
        {
            var userProduct = await _context.UserProducts
                .Include(up => up.Product)
                .Where(up => up.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(up => up.Id == id);

            if (userProduct == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(userProduct));
        }

        // PUT: api/UserProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProduct(Guid id, UserProduct userProduct)
        {
            if (id != userProduct.Id || userProduct.AppUserId != User.GetUserId())
            {
                return BadRequest();
            }

            _context.UserProducts.Entry(_mapper.Map(userProduct)).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/UserProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserProduct>> PostUserProduct(UserProduct userProduct)
        {
            var toAdd = _mapper.Map(userProduct);
            toAdd.AppUserId = User.GetUserId()!.Value;
            var isValidProduct = await _context.Products.AnyAsync(p => p.Id == userProduct.ProductId);
            if (!isValidProduct)
            {
                return NotFound(new Message
                    {Messages = new[] {"Invalid product reference", $"Product {userProduct.ProductId} not found"}});
            }

            _context.UserProducts.Add(toAdd);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUserProduct", new {id = toAdd.Id}, toAdd);
        }

        // DELETE: api/UserProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProduct(Guid id)
        {
            var userProduct = await _context.UserProducts
                .Where(up => up.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(up => up.Id == id);

            if (userProduct == null)
            {
                return NotFound();
            }

            _context.UserProducts.Remove(userProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}