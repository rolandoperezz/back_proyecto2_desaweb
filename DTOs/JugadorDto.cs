using System.ComponentModel.DataAnnotations;

namespace desawebback.DTOs
{
    public class JugadorDto
    {
        [Required(ErrorMessage = "El nombre completo del jugador es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre completo no puede exceder los 150 caracteres.")]
        public string NombreCompleto { get; set; }

        [Range(0, 99, ErrorMessage = "El número debe estar entre 0 y 99.")]
        public int Numero { get; set; }

        [StringLength(50, ErrorMessage = "La posición no puede exceder los 50 caracteres.")]
        public string Posicion { get; set; }

        [Range(100, 250, ErrorMessage = "La estatura debe estar entre 100 y 250 cm.")]
        public int EstaturaCm { get; set; }

        [Range(15, 60, ErrorMessage = "La edad debe estar entre 15 y 60 años.")]
        public int Edad { get; set; }

        [StringLength(50, ErrorMessage = "La nacionalidad no puede exceder los 50 caracteres.")]
        public string Nacionalidad { get; set; }

        [Required(ErrorMessage = "El ID del equipo es obligatorio.")]
        public int EquipoId { get; set; }
    }

    public class JugadorResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public int Numero { get; set; }
        public string Posicion { get; set; }
        public int EstaturaCm { get; set; }
        public int Edad { get; set; }
        public string Nacionalidad { get; set; }
        public EquipoResponseDto Equipo { get; set; } 
    }
}