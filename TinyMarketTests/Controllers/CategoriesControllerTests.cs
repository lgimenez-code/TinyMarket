using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TinyMarketCore.Entities;
using TinyMarketCore.Services;
using TinyMarketDTO.RequestsDTO;
using TinyMarketDTO.ResponseDTO;
using TinyMarketWebApi.Controllers;

namespace TinyMarketTests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
        }

        /// <summary>
        /// Test for Get Categories
        /// </summary>
        [Fact]
        public void Get_ListOfCategories_ReturnsOk()
        {
            List<Category> categories = new List<Category>
            {
                new Category { CategoryId = 1, Name = "Electrónicos", Description = "Productos electrónicos" },
                new Category { CategoryId = 2, Name = "Ropa", Description = "Ropa y accesorios" }
            };

            _mockCategoryService.Setup(service => service.GetAll()).Returns(categories);

            ActionResult result = _controller.Get();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<Category> returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        /// <summary>
        /// Test for Insert Category (with valid model)
        /// </summary>
        [Fact]
        public void Insert_WithValidModel_ReturnsOk()
        {
            CategoryInsertDTO categoryDto = new CategoryInsertDTO
            {
                Name = "Calzado y Zapatos",
                Description = "Calzado y zapatos de cuero finamente seleccionado"
            };

            _mockCategoryService.Setup(service => service.Add(It.IsAny<Category>())).Returns(5);

            ActionResult result = _controller.Insert(categoryDto);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            InsertResponseDTO response = Assert.IsType<InsertResponseDTO>(okResult.Value);
            Assert.Equal(5, response.Id);
            Assert.Contains("Categoría registrada correctamente", response.Message);
        }


        /// <summary>
        /// Test for Insert Category (with invalid model)
        /// </summary>
        [Fact]
        public void Insert_WithInvalidModel_ReturnsBadRequest()
        {
            CategoryInsertDTO categoryDto = new CategoryInsertDTO();

            _controller.ModelState.AddModelError("Name", "El nombre es requerido");

            ActionResult result = _controller.Insert(categoryDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test for Update Category (with valid model)
        /// </summary>
        [Fact]
        public void Update_WithValidModel_ReturnsOk()
        {
            var categoryDto = new CategoryUpdateDTO
            {
                CategoryId = 1,
                Name = "Categoría Actualizada",
                Description = "Descripción actualizada"
            };

            ActionResult result = _controller.Update(categoryDto);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Contains("modificada correctamente", okResult.Value.ToString());
        }

        /// <summary>
        /// Test for Update Category (with invalid model)
        /// </summary>
        [Fact]
        public void Update_WithInvalidModel_ReturnsBadRequest()
        {
            CategoryUpdateDTO categoryDto = null;

            _controller.ModelState.AddModelError("CategoryId", "El Id es obligatorio");

            ActionResult result = _controller.Update(categoryDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Test for Delete Category (with valid ID)
        /// </summary>
        [Fact]
        public void Delete_WithValidId_ReturnsOk()
        {
            int categoryId = 1;

            ActionResult result = _controller.Delete(categoryId);

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Contains("Categoría eliminada correctamente", okResult.Value.ToString());
        }

        /// <summary>
        /// Test for Delete Category (with invalid ID)
        /// </summary>
        [Fact]
        public void Delete_WithZeroId_ReturnsBadRequest()
        {
            int categoryId = 0;

            ActionResult result = _controller.Delete(categoryId);

            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Contains("El Id de la categoría es obligatorio", badRequestResult.Value.ToString());
        }
    }
}
