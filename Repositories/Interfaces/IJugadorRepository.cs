using desawebback.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Repositories.Interfaces
{
    public interface IJugadorRepository
    {
        Task<IEnumerable<Jugador>> GetAllByEquipoAsync(int equipoId, string? searchTerm = null, string? positionFilter = null);
        Task<Jugador> GetByIdAsync(int id);
        Task AddAsync(Jugador jugador);
        Task UpdateAsync(Jugador jugador);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EquipoExistsAsync(int equipoId);
    }
}