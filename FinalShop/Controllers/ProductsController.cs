using FinalShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinalShop.Controllers
{
    public class ProductsController : Controller
    {
        private BlossomBoutiqueContext context {  get; set; }
        public ProductsController(BlossomBoutiqueContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }
    }
}
