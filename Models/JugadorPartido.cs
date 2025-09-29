using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desawebback.Models
{
    public class JugadorPartido
    {
        [Key, Column(Order = 0)]
        public int JugadorId { get; set; }
        [Key, Column(Order = 1)]
        public int PartidoId { get; set; }

        public Jugador Jugador { get; set; }
        public Partido Partido { get; set; }
    }
}