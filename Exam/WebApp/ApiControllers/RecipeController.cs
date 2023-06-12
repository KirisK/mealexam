using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RecipeMapper _mapper;
        private readonly RecipeProductMapper _rpMapper;

        public RecipeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = new RecipeMapper(mapper);
            _rpMapper = new RecipeProductMapper(mapper);
        }

        // GET: api/Recipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes(bool getAllPublicRecipes = false)
        {
            IEnumerable<Domain.App.Recipe> recipes;
            if (User.IsInRole("Admin"))
            {
                recipes = await _context.Recipes
                    .Include(r => r.RecipeProducts)
                    .OrderBy(recipe => recipe.RecipeName)
                    .ToListAsync();
            }
            else
            {
                recipes = await _context.Recipes
                    .Include(r => r.RecipeProducts)
                    .Where(recipe => recipe.AppUserId == User.GetUserId() || recipe.IsPublic && getAllPublicRecipes)
                    .OrderBy(recipe => recipe.RecipeName)
                    .ToListAsync();
            }

            return Ok(recipes.Select(r => _mapper.Map(r)));
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(Guid id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeProducts!)
                .ThenInclude(rp => rp.Product)
                .Where(r => r.IsPublic || r.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(recipe));
        }

        [HttpPost("AddProduct/{recipeId}")]
        public async Task<IActionResult> AddProduct(Guid recipeId, RecipeProduct recipeProduct)
        {
            if (recipeProduct.RecipeId != recipeId)
            {
                return BadRequest();
            }
            
            var recipe = await _context.Recipes
                .Include(r => r.RecipeProducts!)
                .Where(r => r.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe.RecipeProducts != null && recipe.RecipeProducts.Any(rp => rp.ProductId == recipeProduct.ProductId))
            {
                return NoContent();
            }

            _context.RecipesProducts.Add(_rpMapper.Map(recipeProduct)!);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(Guid id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            var currRecipe = await _context.Recipes.AsNoTracking()
                .Where(r => r.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (currRecipe == null)
            {
                return NotFound();
            }
            
            _context.Recipes.Entry(_mapper.Map(recipe)).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return NoContent();
        }

        // POST: api/Recipe
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            var toAdd = _mapper.Map(recipe);
            toAdd.Id = Guid.NewGuid();
            toAdd.AppUserId = User.GetUserId()!.Value;
            _context.Recipes.Add(toAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new {id = toAdd.Id}, toAdd);
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        { 
            var recipe = await _context.Recipes
                .Where(r => r.AppUserId == User.GetUserId() || User.IsInRole("Admin"))
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}