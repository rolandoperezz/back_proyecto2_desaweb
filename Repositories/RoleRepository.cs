using desawebback.Models;
using desawebback.Data;
using Microsoft.EntityFrameworkCore;
using desawebback.Repositories.Interfaces; // Asegúrate de tener este using

namespace desawebback.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        // Método para añadir un rol. Ahora es asincrónico y devuelve un Task.
        public async Task AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        // Este método ya estaba correcto
        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        // Método para verificar si un rol ya existe por su nombre.
        // Es útil para evitar duplicados antes de crearlo.
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Roles.AnyAsync(r => r.Name == name);
        }
    }
}