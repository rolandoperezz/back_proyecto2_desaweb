using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desawebback.Models
{
    public class Partido
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora del partido son obligatorias.")]
        public DateTime FechaHora { get; set; }

        public int MarcadorLocal { get; set; } = 0;
        public int MarcadorVisitante { get; set; } = 0;

        [ForeignKey("EquipoLocal")]
        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; }
        [ForeignKey("EquipoVisitante")]
        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; } 

        public ICollection<JugadorPartido> Roster { get; set; }
    }
}