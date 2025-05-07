using FinalShop.Models;
using FinalShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FinalShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [AllowAnonymous]
        public IActionResult Index(string? searchString)
        {
            // Store the current filter so the view can repopulate the textbox
            ViewData["CurrentFilter"] = searchString;

            var products = _productService.GetAll(searchString);
            return View(products);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound() : View(product);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost, Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            _productService.Create(product);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost, Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            _productService.Update(product);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}