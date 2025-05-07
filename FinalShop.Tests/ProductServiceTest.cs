using System;
using System.Linq;
using FinalShop.Data;
using FinalShop.Models;
using FinalShop.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinalShop.Tests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly BlossomBoutiqueContext _context;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            // 1. Configure In-Memory EF Core
            var opts = new DbContextOptionsBuilder<BlossomBoutiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new BlossomBoutiqueContext(opts);

            // 2. Seed initial data
            _context.Products.AddRange(
                new Product { productID = 1, productName = "Test A", Price = 9.99M, quantity = 5 },
                new Product { productID = 2, productName = "Test B", Price = 19.99M, quantity = 3 }
            );
            _context.SaveChanges();

            // 3. Instantiate your service
            _service = new ProductService(_context);  
        }

        [Fact]
        public void GetAll_Returns_All_Products()
        {
            var all = _service.GetAll();
            Assert.Equal(2, all.Count());
        }

        [Fact]
        public void GetById_Returns_Correct_Product()
        {
            var p = _service.GetById(2);
            Assert.NotNull(p);
            Assert.Equal("Test B", p.productName);
        }

        [Fact]
        public void Create_Adds_New_Product()
        {
            var newProd = new Product { productName = "Test C", Price = 4.99M, quantity = 10 };
            _service.Create(newProd);

            var all = _service.GetAll();
            Assert.Equal(3, all.Count());
            Assert.Contains(all, x => x.productName == "Test C");
        }

        [Fact]
        public void Update_Modifies_Existing_Product()
        {
            var prod = _service.GetById(1);
            prod.Price = 99.99M;
            _service.Update(prod);

            var updated = _service.GetById(1);
            Assert.Equal(99.99M, updated.Price);
        }

        [Fact]
        public void Delete_Removes_Product()
        {
            _service.Delete(1);

            var all = _service.GetAll();
            Assert.Single(all);
            Assert.Null(_service.GetById(1));
        }

        public void Dispose() => _context.Dispose();
    }
}
