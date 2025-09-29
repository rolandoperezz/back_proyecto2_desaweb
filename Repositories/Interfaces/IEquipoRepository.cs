using desawebback.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Repositories.Interfaces
{
    public interface IEquipoRepository
    {
        Task<IEnumerable<Equipo>> GetAllAsync(string? searchTerm = null, string? cityFilter = null);
        Task<Equipo> GetByIdAsync(int id);
        Task AddAsync(Equipo equipo);
        Task UpdateAsync(Equipo equipo);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}