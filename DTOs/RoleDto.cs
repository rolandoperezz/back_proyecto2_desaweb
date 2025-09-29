using System.ComponentModel.DataAnnotations;

namespace desawebback.DTOs
{
    public class RoleDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string Name { get; set; }
    }
}