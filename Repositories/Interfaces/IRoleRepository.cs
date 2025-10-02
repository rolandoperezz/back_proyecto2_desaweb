using desawebback.Models;
using desawebback.DTOs;

namespace desawebback.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Busca el nombre del rol y retorna el primer registro si hay coincidencia
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retorna la primer coincidencia</returns>
        Task<Role?> GetByNameAsync(string name);
        Task AddAsync(Role role);
        Task<bool> ExistsByNameAsync(string name);
        
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}