using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalShop.Data;
using FinalShop.Models;

namespace FinalShop.Views.Shared
{
    public class IndexModel : PageModel
    {
        private readonly FinalShop.Data.BlossomBoutiqueContext _context;

        public IndexModel(FinalShop.Data.BlossomBoutiqueContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products.ToListAsync();
        }
    }
}
