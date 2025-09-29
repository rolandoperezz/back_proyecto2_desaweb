using System.ComponentModel.DataAnnotations;

namespace desawebback.DTOs
{
    public class EquipoDto
    {
        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }
        
        [StringLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres.")]
        public string Ciudad { get; set; }
        public string LogoUrl { get; set; }
    }

    public class EquipoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string LogoUrl { get; set; }
    }
}