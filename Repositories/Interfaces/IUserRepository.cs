using desawebback.Models;

namespace desawebback.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserWithRoleAsync(string username, string password);
        /// <summary>
        /// Busca el usuario en la base de datos y retorna la coincidencia
        /// </summary>
        /// <param name="username">string required</param>
        /// <returns>Retorna el primer registro segun el filtro (WHERE en el SELECT)</returns>
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<List<User>> GetAllWithRolesAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}