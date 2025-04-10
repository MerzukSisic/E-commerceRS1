using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "FIFA 25",
                Description = "Football simulation game",
                Price = 59.99m,
                QuantityInStock = 10,
                PictureUrl = "https://example.com/fifa25.jpg",
                Brand = "EA Sports",
                Type = "Sports",
                PlatformType = Platform.PC
            };

            var productRepoMock = new Mock<IGenericRepository<Product>>();
            productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Repository<Product>()).Returns(productRepoMock.Object);

            var controller = new ProductsController(unitOfWorkMock.Object);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnOk_WhenProductExistsAndUpdated()
        {
            // Arrange
            var productId = 1;
            var newStock = 15;
            var product = new Product
            {
                Id = productId,
                Name = "FIFA 25",
                Description = "Football simulation game",
                Price = 59.99m,
                QuantityInStock = 5,
                PictureUrl = "https://example.com/fifa25.jpg",
                Brand = "EA Sports",
                Type = "Sports",
                PlatformType = Platform.PC
            };

            var productRepoMock = new Mock<IGenericRepository<Product>>();
            productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            productRepoMock.Setup(r => r.Update(It.IsAny<Product>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Repository<Product>()).Returns(productRepoMock.Object);
            unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);

            var controller = new ProductsController(unitOfWorkMock.Object);

            // Act
            var result = await controller.UpdateStock(productId, newStock);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(newStock, product.QuantityInStock);
        }
    }
}
