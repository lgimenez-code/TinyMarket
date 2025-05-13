using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;

namespace TinyMarketDTO.RequestsDTO
{
    public class ProductInsertDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal? Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El Stock debe ser cero o mayor a cero.")]
        public int? Stock { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Categoría")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Proveedor")]
        public int? SupplierId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Status { get; set; }


        /// <summary>
        /// convierte un DTO en una entidad
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Product FromProductDTO(ProductInsertDTO dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                ExpirationDate = dto.ExpirationDate,
                Status = dto.Status
            };
        }
    }
}
