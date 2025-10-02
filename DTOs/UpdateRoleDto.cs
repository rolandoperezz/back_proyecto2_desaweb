using System.ComponentModel.DataAnnotations;

namespace desawebback.DTOs
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public required string Name { get; set; }
    }
}