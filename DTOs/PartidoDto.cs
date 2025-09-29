using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace desawebback.DTOs
{
    public class CrearPartidoDto
    {
        [Required(ErrorMessage = "La fecha y hora del partido son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El ID del equipo local es obligatorio.")]
        public int EquipoLocalId { get; set; }

        [Required(ErrorMessage = "El ID del equipo visitante es obligatorio.")]
        public int EquipoVisitanteId { get; set; }
    }

    public class ActualizarMarcadorPartidoDto
    {
        public int MarcadorLocal { get; set; }
        public int MarcadorVisitante { get; set; }
    }

    public class PartidoResponseDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int MarcadorLocal { get; set; }
        public int MarcadorVisitante { get; set; }
        public EquipoResponseDto EquipoLocal { get; set; }
        public EquipoResponseDto EquipoVisitante { get; set; }
        public ICollection<JugadorResponseDto> RosterLocal { get; set; } 
        public ICollection<JugadorResponseDto> RosterVisitante { get; set; } 
    }
}