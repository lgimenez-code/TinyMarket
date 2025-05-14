using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TinyMarketCore.Entities;
using TinyMarketCore.Services;
using TinyMarketDTO.RequestsDTO;
using TinyMarketDTO.ResponseDTO;
using TinyMarketWebApi.Controllers;

namespace TinyMarketTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        /// <summary>
        /// Test for Get Products
        /// </summary>
        [Fact]
        public void Get_ListOfProducts_ReturnsOk()
        {
            List<Product> expectedProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Producto 1" },
                new Product { ProductId = 2, Name = "Producto 2" }
            };

            _mockService.Setup(service => service.GetAll()).Returns(expectedProducts);

            ActionResult result = _controller.Get();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<Product> products = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(2, products.Count);
        }

        /// <summary>
        /// Test for Insert Products (with valid model)
        /// </summary>
        [Fact]
        public void Insert_WithValidModel_ReturnsOk()
        {
            ProductInsertDTO dto = new ProductInsertDTO
            {
                Name = "Producto Test",
                Description = "Descripción Test",
                Price = 65000,
                Stock = 10,
                CategoryId = 1,
                SupplierId = 3,
                ExpirationDate = new DateTime(2029, 12, 31),
            };

            Product expectedProduct = ProductInsertDTO.FromProductDTO(dto);
            _mockService.Setup(service => service.Add(It.IsAny<Product>())).Returns(1);
            ActionResult result = _controller.Insert(dto);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            InsertResponseDTO response = Assert.IsType<InsertResponseDTO>(okResult.Value);
            Assert.Equal(1, response.Id);
            Assert.Equal("Producto registrado correctamente", response.Message);
        }

        /// <summary>
        /// Test for Insert Products (with invalid model)
        /// </summary>
        [Fact]
        public void Insert_WithInvalidModel_ReturnsBadRequest()
        {
            ProductInsertDTO dto = null;
            _controller.ModelState.AddModelError("Name", "El nombre es requerido");
            ActionResult result = _controller.Insert(dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test for Update Products (with valid model)
        /// </summary>
        [Fact]
        public void Update_WithValidModel_ReturnsOk()
        {
            // Arrange
            var dto = new ProductUpdateDTO
            {
                ProductId = 1,
                Name = "Producto 4",
                Description = "Descripción de prueba",
                Price = 1500,
                Stock = 15,
                CategoryId = 1,
                SupplierId = 3,
                ExpirationDate = new DateTime(2029, 12, 31),
                Status = "A"
            };

            _mockService.Setup(service => service.Update(It.IsAny<Product>()));
            ActionResult result = _controller.Update(dto);
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Producto modificado correctamente", okResult.Value);
        }

        /// <summary>
        /// Test for Update Products (with invalid model)
        /// </summary>
        [Fact]
        public void Update_WithInvalidModel_ReturnsBadRequest()
        {
            ProductUpdateDTO dto = null;
            _controller.ModelState.AddModelError("ProductId", "El Id es obligatorio");
            ActionResult result = _controller.Update(dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test for Delete Products (with valid ID)
        /// </summary>
        [Fact]
        public void Delete_WithValidId_ReturnsOk()
        {
            int validId = 1;
            _mockService.Setup(service => service.Delete(validId));

            ActionResult result = _controller.Delete(validId);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Producto eliminado correctamente", okResult.Value);
        }

        /// <summary>
        /// Test for Delete Products (with invalid ID)
        /// </summary>
        [Fact]
        public void Delete_ReturnsBadRequest_WhenIdIsZero()
        {
            int invalidId = 0;
            ActionResult result = _controller.Delete(invalidId);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
