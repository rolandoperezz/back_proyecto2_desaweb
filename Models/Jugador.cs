using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desawebback.Models
{
    public class Jugador
    {
        [Key]
        public int Id { get; set; }

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

        [ForeignKey("Equipo")]
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } 
        
        public ICollection<JugadorPartido> PartidosJugados { get; set; }
    }
}