using desawebback.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Repositories.Interfaces
{
    public interface IPartidoRepository
    {
        Task<IEnumerable<Partido>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Partido> GetByIdAsync(int id);
        Task AddAsync(Partido partido);
        Task UpdateAsync(Partido partido);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EquipoExistsAsync(int equipoId);
        Task AddPlayersToRosterAsync(int partidoId, int equipoId, List<int> jugadorIds);
        Task<IEnumerable<Jugador>> GetRosterByPartidoAndEquipoAsync(int partidoId, int equipoId);
        Task UpdateScoreAsync(int partidoId, int marcadorLocal, int marcadorVisitante);

    }
}