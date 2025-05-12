using System.ComponentModel.DataAnnotations;
using TinyMarketCore.Entities;


namespace TinyMarketDTO.RequestsDTO
{
    public class CategoryUpdateDTO
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Description { get; set; }



        public static Category FromCategoryDTO(CategoryUpdateDTO dto)
        {
            return new Category
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
