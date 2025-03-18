using e_commerce.Controllers;
using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ornae_commerce
{
    [TestFixture]
    public class ProductControllerTests
    {
        private AppDbContext _context;
        private Mock<ILogger<ProductController>> _mockLogger;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ECommerceTest")
                .Options;

            _context = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_context);

            ClearDatabase();
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void ClearDatabase()
        {
            _context.Products.RemoveRange(_context.Products);
            _context.Categories.RemoveRange(_context.Categories);
            _context.SaveChanges();
        }

        private void SeedDatabase()
        {
            var category = new Categories
            {
                CategoryName = "Electronics"
            };

            var product = new Products
            {
                ProductId = 1,
                ProductName = "Laptop",
                Description = "A high-performance laptop",
                Price = 1000m,
                Stock = 10,
                CategoryId = 1,
                ProductImage = "laptop.jpg",
                Category = category
            };

            _context.Categories.Add(category);
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = 99;

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostProduct_ReturnsCreatedAtActionResult_WithProduct()
        {
            // Arrange
            var productDto = new ProductDTO
            {
                ProductId = 2,
                ProductName = "Smartphone",
                Description = "A new smartphone",
                Price = 500m,
                Stock = 20,
                CategoryId = 1,
                ProductImage = "smartphone.jpg"
            };

            // Act
            var result = await _controller.PostProduct(productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult.Value, Is.InstanceOf<Products>());
        }

        

        [Test]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductDeleted()
        {
            // Arrange
            var productId = 1;

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = 99;

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

