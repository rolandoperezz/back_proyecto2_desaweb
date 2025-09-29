using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace desawebback.Models
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres.")]
        public string Ciudad { get; set; }

        public string LogoUrl { get; set; } 

        public ICollection<Jugador> Jugadores { get; set; }
        
        public ICollection<Partido> PartidosLocal { get; set; }
        public ICollection<Partido> PartidosVisitante { get; set; }
    }
}