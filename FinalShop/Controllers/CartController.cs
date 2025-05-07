// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using FinalShop.Extensions;
using FinalShop.Models;
using FinalShop.Services;

namespace FinalShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly IProductService _productService;

        public CartController(IProductService productService)
            => _productService = productService;

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                       ?? new List<CartItem>();
            return View(cart);
        }

        // POST: /Cart/Add/5
        [HttpPost]
        public IActionResult Add(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();

            var cart = HttpContext.Session
                         .GetObject<List<CartItem>>(CartSessionKey)
                      ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(ci => ci.productID == id);
            if (existing != null)
            {
                existing.quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    productID = product.productID,
                    productName = product.productName,
                    price = product.Price,
                    quantity = 1
                });
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove/5
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session
                         .GetObject<List<CartItem>>(CartSessionKey)
                      ?? new List<CartItem>();

            var item = cart.FirstOrDefault(ci => ci.productID == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObject(CartSessionKey, cart);
            }

            return RedirectToAction("Index");
        }
    }
}
