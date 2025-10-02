using desawebback.Models;
using desawebback.Data;
using Microsoft.EntityFrameworkCore;
using desawebback.Repositories.Interfaces; 
using desawebback.DTOs; 
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

        // Agregar al final de la clase, antes del cierre }
public async Task<List<Role>> GetAllAsync()
{
    return await _context.Roles
        .OrderBy(r => r.Name)
        .ThenBy(r => r.Id)
        .ToListAsync();
}



public async Task<Role?> GetByIdAsync(int id)
{
    return await _context.Roles.FindAsync(id);
}

public async Task UpdateAsync(Role role)
{
    _context.Roles.Update(role);
    await _context.SaveChangesAsync();
}

public async Task DeleteAsync(int id)
{
    var role = await GetByIdAsync(id);
    if (role != null)
    {
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
    }
}
    }
}