using FinalShop.Data;
using FinalShop.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalShop.Services
{
    public class ProductService : IProductService
    {
        private readonly BlossomBoutiqueContext _context;

        public ProductService(BlossomBoutiqueContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAll(string? searchString = null)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(p =>
                    p.productName.Contains(searchString));
            }
            return query.ToList();
        }

        public IEnumerable<Product> GetAll() => _context.Products.ToList();

        public Product? GetById(int id) => _context.Products.FirstOrDefault(p => p.productID == id);

        public void Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}