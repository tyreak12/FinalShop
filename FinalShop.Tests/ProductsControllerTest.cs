using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FinalShop.Controllers;
using FinalShop.Models;
using FinalShop.Services;

namespace FinalShop.Tests
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public void Index_ReturnsViewWithAllProducts()
        {
            // Arrange
            var sample = new List<Product> {
                new Product { productID = 1, productName = "A", Price = 1, quantity = 1 },
                new Product { productID = 2, productName = "B", Price = 2, quantity = 2 }
            };
            _mockService.Setup(s => s.GetAll()).Returns(sample);

            // Act
            var result = _controller.Index();

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(view.Model);
            Assert.Equal(2, ((List<Product>)model).Count);
        }

        [Fact]
        public void Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetById(999)).Returns((Product)null);

            // Act
            var result = _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_WithValidId_ReturnsViewWithProduct()
        {
            // Arrange
            var prod = new Product { productID = 5, productName = "Z", Price = 9.99M, quantity = 3 };
            _mockService.Setup(s => s.GetById(5)).Returns(prod);

            // Act
            var result = _controller.Details(5);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(prod, view.Model);
        }

        [Fact]
        public void Create_GET_ReturnsView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_POST_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newProd = new Product { productName = "New", Price = 3.50M, quantity = 4 };
            // ModelState is valid by default

            // Act
            var result = _controller.Create(newProd);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _mockService.Verify(s => s.Create(newProd), Times.Once);
        }

        [Fact]
        public void Create_POST_InvalidModel_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Price", "Required");
            var badProd = new Product();

            // Act
            var result = _controller.Create(badProd);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(badProd, view.Model);
        }

        [Fact]
        public void Edit_GET_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetById(10)).Returns((Product)null);

            // Act
            var result = _controller.Edit(10);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_GET_ValidId_ReturnsViewWithProduct()
        {
            // Arrange
            var prod = new Product { productID = 3, productName = "X", Price = 5.00M, quantity = 2 };
            _mockService.Setup(s => s.GetById(3)).Returns(prod);

            // Act
            var result = _controller.Edit(3);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(prod, view.Model);
        }

        [Fact]
        public void Edit_POST_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var prod = new Product { productID = 3, productName = "X", Price = 5.00M, quantity = 2 };

            // Act
            var result = _controller.Edit(prod);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _mockService.Verify(s => s.Update(prod), Times.Once);
        }

        [Fact]
        public void Edit_POST_InvalidModel_ReturnsView()
        {
            // Arrange
            var prod = new Product { productID = 3 };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Edit(prod);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(prod, view.Model);
        }

        [Fact]
        public void Delete_POST_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(2);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _mockService.Verify(s => s.Delete(2), Times.Once);
        }
    }
}
