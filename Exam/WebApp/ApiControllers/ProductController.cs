using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Public.DTO.v1;
using Public.DTO.v1.Mappers;
using WebApp.Extensions;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for the product
    /// </summary>
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductMapper _mapper;

        /// <summary>
        /// Constructor of product controller
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ProductController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = new ProductMapper(mapper);
        }

        // GET: api/Products
        /// <summary>
        /// Get list of products that belong to user
        /// </summary>
        /// <param name="filterOutOwned"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(bool filterOutOwned = false)
        {

            List<Domain.App.Product> products; 
            if (filterOutOwned)
            {
                products = await _context.Products
                    .Where(product => _context.UserProducts
                        .Where(up => up.AppUserId == User.GetUserId())
                        .Count(up => up.ProductId != product.Id) == 0)
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products
                    .ToListAsync();
            } 
            
            return Ok(products.Select(p => _mapper.Map(p)));
        }

        // GET: api/Products/5
        /// <summary>
        /// Get list of products by their id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.RecipeProducts)
                .Include(p => p.UserProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Editing of products enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Products.Entry(_mapper.Map(product)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add new products to general list of products, enabled only for admin
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var toAdd = _mapper.Map(product);
            toAdd.Id = Guid.NewGuid();
            _context.Products.Add(toAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = toAdd.Id }, toAdd);
        }

        // DELETE: api/Products/5
        /// <summary>
        /// Deleting product from general list of products, enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
