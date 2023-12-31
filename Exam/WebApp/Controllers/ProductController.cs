using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Public.DTO.v1;
using WebApp.HttpClient;
using WebApp.ViewModels.Product;

namespace WebApp.Controllers
{
    /// <summary>
    /// UI product controller
    /// </summary>
    public class ProductController : Controller
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IProductClient _productClient;

        /// <summary>
        /// Constructor for product controller
        /// </summary>
        /// <param name="jwtHelper"></param>
        /// <param name="productClient"></param>
        public ProductController(JwtHelper jwtHelper, IProductClient productClient)
        {
            _jwtHelper = jwtHelper;
            _productClient = productClient;
        }

        // GET: Product
        /// <summary>
        /// List of products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            if (User.IsInRole("Admin"))
            {
                var resp = await _productClient.GetAllProducts(_jwtHelper.GetJwt(User));
                if (resp.IsSuccessful)
                {
                    model.Products = resp.Value!;
                    return View(model);
                }

                return Problem();
            }

            var userProductsResp = await _productClient.GetUserProducts(_jwtHelper.GetJwt(User));
            if (userProductsResp.IsSuccessful)
            {
                model.UserProducts = userProductsResp.Value!;
                return View(model);
            }

            return NotFound();
        }

        // GET: Product/Create
        /// <summary>
        /// Creation of new product
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creation of new product, enabled only for admin
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var resp = await _productClient.AddProduct(_jwtHelper.GetJwt(User), product);
            if (resp.IsSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Edit/5
        /// <summary>
        /// Editing product, enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productClient.GetProduct(_jwtHelper.GetJwt(User), id.Value);
            if (product.IsSuccessful)
            {
                return View(product.Value!);
            }

            return NotFound();
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Editing product, enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var resp = await _productClient.UpdateProduct(_jwtHelper.GetJwt(User), product);
            if (resp.IsSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Delete/5
        /// <summary>
        /// Deleting of product from list, enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resp = await _productClient.GetProduct(_jwtHelper.GetJwt(User), id.Value);
            if (resp.IsSuccessful)
            {
                return View(resp.Value!);
            }

            return NotFound();
        }

        // POST: Product/Delete/5
        /// <summary>
        /// Deleting of product from list, enabled only for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resp = await _productClient.DeleteProduct(_jwtHelper.GetJwt(User), id);
            if (resp.IsSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }

            return Problem(String.Join("\n", resp.ErrorMessage?.Messages ?? new[] {$"could not delete product {id}"}));
        }
    }
}