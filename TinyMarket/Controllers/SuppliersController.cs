using Microsoft.AspNetCore.Mvc;
using TinyMarketCore.Entities;
using TinyMarketCore.Services;
using TinyMarketDTO.RequestsDTO;
using TinyMarketDTO.ResponseDTO;

namespace TinyMarketWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Obtiene los Proveedores
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /suppliers
        ///    
        /// </remarks>
        /// <returns>Una lista de Proveedores</returns>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            IEnumerable<Supplier> suppliers = _supplierService.GetAll();

            return Ok(suppliers);
        }

        /// <summary>
        /// Crea un nuevo Proveedor.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /suppliers
        ///     {
        ///         "Name": "Lucas Gimenez",  
        ///         "Address": "Carlos Guillaume 5413",
        ///         "Phone": "3517794857",
        ///         "Email": "lgimenez@gmail.com",
        ///         "ProvinceId": 4,
        ///     }
        /// </remarks>
        /// <returns>ID registrado correctamente.</returns>
        /// <response code="200">ID registrado correctamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Insert([FromBody] SupplierInsertDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Supplier supplier = SupplierInsertDTO.FromSupplierInsertDTO(dto);

                int id = _supplierService.Add(supplier);

                return Ok(new InsertResponseDTO() { Message = "Proveedor registrado correctamente", Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Modifica un Proveedor existente
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     PUT /suppliers
        ///     {
        ///         "SupplierId": 45,
        ///         "Name": "Lucas Gimenez",  
        ///         "Address": "Carlos Guillaume 5413",
        ///         "Phone": "3517794857",
        ///         "Email": "lgimenez@gmail.com",
        ///         "ProvinceId": 4,
        ///         "Status": "R"
        ///     }
        /// </remarks>
        /// <returns>mensaje de éxito</returns>
        /// <response code="200">Mensaje de éxito</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Update([FromBody] SupplierUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Supplier supplier = SupplierUpdateDTO.FromSupplierUpdateDTO(dto);

                _supplierService.Update(supplier);

                return Ok("Proveedor modificado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un Proveedor existente
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     DELETE /suppliers/5
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
                    return BadRequest("El Id del Proveedor es obligatorio.");
                }
                _supplierService.Delete(id);
                return Ok("Proveedor eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
