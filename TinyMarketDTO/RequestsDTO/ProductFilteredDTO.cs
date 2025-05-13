using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;

namespace TinyMarketDTO.RequestsDTO
{
    public class ProductFilteredDTO : IValidatableObject
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo no puede ser negativo.")]
        public decimal? MinPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El precio máximo no puede ser negativo.")]
        public decimal? MaxPrice { get; set; }
        public int? Stock { get; set; }
        public string? Status { get; set; }

        /// <summary>
        /// validacion de apoyo para los Montos Máximos y Mínimos
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
            {
                yield return new ValidationResult(
                    "El precio mínimo no puede ser mayor que el precio máximo.",
                    new[] { nameof(MinPrice) }
                );
            }
        }

        /// <summary>
        /// convierte un DTO en una entidad
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ProductFiltered FromProductFilteredDTO(ProductFilteredDTO dto)
        {
            return new ProductFiltered()
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                MinPrice = dto.MinPrice,
                MaxPrice = dto.MaxPrice,
                Stock = dto.Stock,
                Status = dto.Status,
            };
        }
    }
}
