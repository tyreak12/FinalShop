using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FinalShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            int visits = HttpContext.Session.GetInt32("VisitCount") ?? 0;
            visits++;
            HttpContext.Session.SetInt32("VisitCount", visits);

            ViewData["VisitCount"] = visits;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

