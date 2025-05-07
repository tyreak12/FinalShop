using FinalShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalShop.Data
{
    public class BlossomBoutiqueContext
        : IdentityDbContext<ApplicationUser>
    {
        public BlossomBoutiqueContext(DbContextOptions<BlossomBoutiqueContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed your roles or other identity data here if you want
            builder.Entity<Product>().HasData(
                new Product { productID = 1, productName = "Rose Bouquet", productDescription = "A beautiful bouquet of roses.", Price = 29.99M },
                new Product { productID = 2, productName = "Tulip Arrangement", productDescription = "A stunning arrangement of tulips.", Price = 19.99M },
                new Product { productID = 3, productName = "Orchid Plant", productDescription = "A lovely orchid plant.", Price = 39.99M }
            );
        }
    }
}
