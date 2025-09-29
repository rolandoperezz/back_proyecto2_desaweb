using desawebback.DTOs;
using desawebback.Models; // Usamos el modelo aquí para el retorno, si queremos la entidad completa
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Services.Interfaces
{
    public interface IAdminService
    {
        // Equipos
        Task<IEnumerable<EquipoResponseDto>> GetEquiposAsync(string searchTerm = null, string cityFilter = null);
        Task<EquipoResponseDto> GetEquipoByIdAsync(int id);
        Task<EquipoResponseDto> CreateEquipoAsync(EquipoDto equipoDto);
        Task<bool> UpdateEquipoAsync(int id, EquipoDto equipoDto);
        Task<bool> DeleteEquipoAsync(int id);

        // Jugadores
        Task<IEnumerable<JugadorResponseDto>> GetJugadoresByEquipoAsync(int equipoId, string searchTerm = null, string positionFilter = null);
        Task<JugadorResponseDto> GetJugadorByIdAsync(int id);
        Task<JugadorResponseDto> CreateJugadorAsync(JugadorDto jugadorDto);
        Task<bool> UpdateJugadorAsync(int id, JugadorDto jugadorDto);
        Task<bool> DeleteJugadorAsync(int id);

        // Partidos
        Task<IEnumerable<PartidoResponseDto>> GetPartidosAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PartidoResponseDto> GetPartidoByIdAsync(int id);
        Task<PartidoResponseDto> CreatePartidoAsync(CrearPartidoDto partidoDto);
        Task<bool> UpdatePartidoAsync(int id, CrearPartidoDto partidoDto); 
        Task<bool> DeletePartidoAsync(int id);
        Task<bool> AssignRosterToPartidoAsync(int partidoId, int equipoId, List<int> jugadorIds);
        Task<bool> UpdatePartidoMarcadorAsync(int partidoId, ActualizarMarcadorPartidoDto marcadorDto);

    }
}