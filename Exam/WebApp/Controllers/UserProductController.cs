using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Microsoft.AspNetCore.Authorization;
using Public.DTO.v1;
using Public.DTO.v1.Mappers;
using WebApp.Extensions;
using WebApp.HttpClient;
using WebApp.ViewModels.UserProduct;

namespace WebApp.Controllers
{
    public class UserProductController : Controller
    {
        private readonly IProductClient _productClient;
        private readonly JwtHelper _jwtHelper;

        public UserProductController(IProductClient productClient, JwtHelper jwtHelper)
        {
            _productClient = productClient;
            _jwtHelper = jwtHelper;
        }
        

        // GET: UserProduct/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new CreateViewModel();
            model.Products = await GetProductsSelectList();
            model.UserProduct = new Public.DTO.v1.UserProduct {AppUserId = User.GetUserId()!.Value};
            return View(model);
        }

        // POST: UserProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resp = await _productClient.AddUserProduct(_jwtHelper.GetJwt(User), model.UserProduct);
                if (resp.IsSuccessful)
                {
                    return RedirectToAction(nameof(Index), nameof(Product));
                }
            }
            model.Products = await GetProductsSelectList();
            return View(model);
        }

        private async Task<List<SelectListItem>> GetProductsSelectList()
        {
            var products = await _productClient.GetAllProducts(_jwtHelper.GetJwt(User), true);
            if (products.IsSuccessful)
            {
                return products.Value!
                    .Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.ProductName})
                    .ToList();
            }

            return new List<SelectListItem>();
        }

        // GET: UserProduct/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProduct = await _productClient.GetUserProduct(_jwtHelper.GetJwt(User), id.Value);
            if (!userProduct.IsSuccessful)
            {
                return NotFound();
            }
            return View(userProduct.Value!);
        }

        // POST: UserProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserProduct userProduct)
        {
            if (id != userProduct.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userProduct);
            }

            if (userProduct.AvailableAmount <= 0)
            {
                var deleteResp = await _productClient.DeleteUserProduct(_jwtHelper.GetJwt(User), userProduct.Id);
                if (deleteResp.IsSuccessful)
                {
                    return RedirectToAction(nameof(Index), nameof(Product));
                }

                return Problem($"Could not remove product {userProduct.Id}");
            }
            
            var resp = await _productClient.UpdateUserProduct(_jwtHelper.GetJwt(User), userProduct);
            if (resp.IsSuccessful)
            {
                return RedirectToAction(nameof(Index), nameof(Product));
            }
            return View(userProduct);
        }
    }
}
