

using System.ComponentModel.DataAnnotations;
using TinyMarketCore.Entities;

namespace TinyMarketDTO.RequestsDTO
{
    public class CategoryInsertDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Description { get; set; }

        public static Category FromCategoryDTO(CategoryInsertDTO dto)
        {
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
