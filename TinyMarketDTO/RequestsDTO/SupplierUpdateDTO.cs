using System.ComponentModel.DataAnnotations;
using TinyMarketCore.Entities;

namespace TinyMarketDTO.RequestsDTO
{
    public class SupplierUpdateDTO
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int? SupplierId { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "La dirección no puede superar los 500 caracteres.")]
        public string? Address { get; set; }
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La Provincia es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Provincia")]
        public int? ProvinceId { get; set; }
        public string? Status { get; set; }


        /// <summary>
        /// convierte un DTO en una entidad
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Supplier FromSupplierUpdateDTO(SupplierUpdateDTO dto)
        {
            return new Supplier()
            {
                SupplierId = dto.SupplierId,
                Name = dto.Name,
                Address = dto.Address ?? "",
                Phone = dto.Phone ?? "",
                Email = dto.Email ?? "",
                ProvinceId = dto.ProvinceId,
                Status = dto?.Status ?? "R",
            };
        }
    }
}
