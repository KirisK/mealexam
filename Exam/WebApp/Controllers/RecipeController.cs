using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Public.DTO.v1;
using WebApp.Extensions;
using WebApp.HttpClient;
using WebApp.ViewModels.Recipe;
using Product = Public.DTO.v1.Product;

namespace WebApp.Controllers
{
    /// <summary>
    /// UI controller for recipe
    /// </summary>
    public class RecipeController : Controller
    {
        private readonly IRecipeClient _recipeClient;
        private readonly IProductClient _productClient;
        private readonly JwtHelper _jwtHelper;

        /// <summary>
        /// Constructor for recipe controller
        /// </summary>
        /// <param name="recipeClient"></param>
        /// <param name="productClient"></param>
        /// <param name="jwtHelper"></param>
        public RecipeController(
            IRecipeClient recipeClient,
            IProductClient productClient,
            JwtHelper jwtHelper)
        {
            _recipeClient = recipeClient;
            _jwtHelper = jwtHelper;
            _productClient = productClient;
        }

        // GET: Recipe
        /// <summary>
        /// List of recipes
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userRecipes = await _recipeClient.GetAllRecipes(_jwtHelper.GetJwt(User));
            if (userRecipes.IsSuccessful)
            {
                return View(userRecipes.Value);
            }

            return NotFound();
        }

        // GET: Recipe/Details/5
        /// <summary>
        /// Recipe details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipeClient.GetRecipe(_jwtHelper.GetJwt(User), id.Value);
            if (!recipe.IsSuccessful)
            {
                return NotFound();
            }

            var model = new DetailsViewModel();
            model.Recipe = recipe.Value!;
            model.UserProducts = await GetUserProducts();

            return View(model);
        }

        private async Task<IEnumerable<UserProduct>> GetUserProducts()
        {
            var resp = await _productClient.GetUserProducts(_jwtHelper.GetJwt(User));
            return resp.IsSuccessful ? resp.Value! : Array.Empty<UserProduct>();
        }

        /// <summary>
        /// Adding products to recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipeClient.GetRecipe(_jwtHelper.GetJwt(User), id.Value);
            if (!recipe.IsSuccessful)
            {
                return NotFound();
            }

            var model = new AddProductViewModel();
            model.Recipe = recipe.Value!;
            model.Products = await GetAllProductsSelectItems();
            model.RecipeProduct = new RecipeProduct {RecipeId = id.Value};

            return View(model);
        }

        /// <summary>
        /// Adding products to recipe using additional view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resp = await _recipeClient.AddRecipeProduct(_jwtHelper.GetJwt(User), model.RecipeProduct);
                if (resp.IsSuccessful)
                {
                    return RedirectToAction(nameof(Details), new { id = model.RecipeProduct.RecipeId });
                }
            }

            model.Recipe = (await _recipeClient.GetRecipe(_jwtHelper.GetJwt(User), model.RecipeProduct.RecipeId))
                .Value!;
            model.RecipeProduct = new RecipeProduct {RecipeId = model.RecipeProduct.RecipeId};
            return View(model);
        }

        private async Task<List<SelectListItem>> GetAllProductsSelectItems()
        {
            var products = (await _productClient.GetAllProducts(_jwtHelper.GetJwt(User))).Value ??
                           Array.Empty<Product>();
            return products
                .Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.ProductName})
                .ToList();
        }

        // GET: Recipe/Create
        /// <summary>
        /// Create new recipe
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var recipe = new Recipe();
            recipe.AppUserId = User.GetUserId()!.Value;
            
            return View(recipe);
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create new recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                var resp = await _recipeClient.AddRecipe(_jwtHelper.GetJwt(User), recipe);
                if (resp.IsSuccessful)
                {
                    return RedirectToAction(nameof(Details), new { id = resp.Value!.Id });
                }
            }

            return View(recipe);
        }

        // GET: Recipe/Edit/5
        /// <summary>
        /// Edit recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipeClient.GetRecipe(_jwtHelper.GetJwt(User), id.Value);
            if (!recipe.IsSuccessful || recipe.Value == null)
            {
                return NotFound();
            }

            return View(recipe.Value);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit recipe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(recipe);
            }
            
            var resp = await _recipeClient.UpdateRecipe(_jwtHelper.GetJwt(User), recipe);
            if (resp.IsSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipe/Delete/5
        /// <summary>
        /// Delete recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resp = await _recipeClient.GetRecipe(_jwtHelper.GetJwt(User), id.Value);
            if (!resp.IsSuccessful)
            {
                return NotFound();
            }

            return View(resp.Value!);
        }

        // POST: Recipe/Delete/5
        /// <summary>
        /// Confirm deleting of recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resp = await _recipeClient.DeleteAsync(_jwtHelper.GetJwt(User), id);
            
            return resp.IsSuccessful ? RedirectToAction(nameof(Index)) : Problem("Can't delete?");
        }
    }
}