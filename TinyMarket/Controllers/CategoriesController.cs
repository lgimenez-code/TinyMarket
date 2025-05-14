using Microsoft.AspNetCore.Mvc;
using TinyMarketCore.Entities;
using TinyMarketCore.Services;
using TinyMarketDTO.RequestsDTO;
using TinyMarketDTO.ResponseDTO;

namespace TinyMarketWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Obtiene las Categorías
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /categories
        ///    
        /// </remarks>
        /// <returns>Una lista de categorías</returns>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            IEnumerable<Category> categories = _categoryService.GetAll();

            return Ok(categories);
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /categories
        ///     {
        ///         "Name": "Categoría de prueba",
        ///         "Description": "Descripción de prueba"
        ///     }
        /// </remarks>
        /// <returns>ID registrado correctamente.</returns>
        /// <response code="200">ID registrado correctamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Insert([FromBody] CategoryInsertDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Category category = CategoryInsertDTO.FromCategoryDTO(dto);

                int id = _categoryService.Add(category);

                return Ok(new InsertResponseDTO() { Message = "Categoría registrada correctamente", Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Modifica una Categoría existente
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     PUT /categories
        ///     {
        ///         "CategoryId": 6,
        ///         "Name": "nombre de la categoría",  
        ///         "Description": "descripción de prueba",  
        ///     }
        /// </remarks>
        /// <returns>mensaje de éxito</returns>
        /// <response code="200">Mensaje de éxito</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Update([FromBody] CategoryUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Category category = CategoryUpdateDTO.FromCategoryDTO(dto);

                _categoryService.Update(category);

                return Ok("Categoría modificada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina una Categoría existente
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     DELETE /categories/5
        ///    
        /// </remarks>
        /// <returns>mensaje de éxito</returns>
        /// <response code="200">Mensaje de éxito</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("El Id de la categoría es obligatorio.");
                }
                _categoryService.Delete(id);
                return Ok("Categoría eliminada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
