using Microsoft.AspNetCore.Mvc;
using TinyMarketCore.Entities;
using TinyMarketCore.Services;
using TinyMarketDTO.RequestsDTO;
using TinyMarketDTO.ResponseDTO;

namespace TinyMarketWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene los Productos
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///     GET /products
        ///    
        /// </remarks>
        /// <returns>Una lista de productos</returns>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            IEnumerable<Product> products = _productService.GetAll();

            return Ok(products);
        }

        /// <summary>
        /// Obtiene los Productos con los filtros seleccionados
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /products/filtered
        ///     {
        ///         "Name": "Producto de prueba",
        ///         "MinPrice": 100.00,
        ///         "MaxPrice": 9000.00,
        ///         "Stock": 1,
        ///         "CategoryId": 2,
        ///         "SupplierId": 3,
        ///         "ExpirationDate": "2023-12-31",
        ///         "Status": "R"
        ///     }
        /// </remarks>
        /// <returns>Una lista de productos</returns>
        /// <returns></returns>
        [HttpPost("filtered")]
        public ActionResult Get([FromBody] ProductFilteredDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ProductFiltered product = ProductFilteredDTO.FromProductFilteredDTO(dto);

                IEnumerable<Product> products = _productService.GetProductsFiltered(product);

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo Producto.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /products
        ///     {
        ///         "Name": "Producto de prueba",
        ///         "Description": "Descripción de prueba",
        ///         "Price": 100.00,
        ///         "Stock": 10,
        ///         "CategoryId": 2,
        ///         "SupplierId": 3,
        ///         "ExpirationDate": "2023-12-31",
        ///         "Status": "R"
        ///     }
        /// </remarks>
        /// <returns>ID registrado correctamente.</returns>
        /// <response code="200">ID registrado correctamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Insert([FromBody] ProductInsertDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            
                Product product = ProductInsertDTO.FromProductDTO(dto);

                int id = _productService.Add(product);

                return Ok(new InsertResponseDTO() { Message = "Producto registrado correctamente", Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Modifica un Producto existente
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     PUT /products
        ///     {
        ///         "ProductId": 1,
        ///         "Name": "Producto de prueba",
        ///         "Description": "Descripción de prueba",
        ///         "Price": 100.00,
        ///         "Stock": 10,
        ///         "CategoryId": 2,
        ///         "SupplierId": 3,
        ///         "ExpirationDate": "2023-12-31",
        ///         "Status": "R" 
        ///     }
        /// </remarks>
        /// <returns>mensaje de éxito</returns>
        /// <response code="200">Mensaje de éxito</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Update([FromBody] ProductUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Product product = ProductUpdateDTO.FromProductDTO(dto);

                _productService.Update(product);

                return Ok("Producto modificado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un Producto existente
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
                _productService.Delete(id);
                return Ok("Producto eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
