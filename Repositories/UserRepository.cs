using desawebback.Models;
using desawebback.Repositories.Interfaces;
using desawebback.Data;
using Microsoft.EntityFrameworkCore;
using desawebback.DTOs; 

namespace desawebback.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetUserWithRoleAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;
            return user;
        }

        /// <summary>
        /// Busca el usuario en la base de datos y retorna la coincidencia
        /// </summary>
        /// <param name="username">string required</param>
        /// <returns>Retorna el primer registro segun el filtro (WHERE en el SELECT)</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

    public async Task<List<User>> GetAllWithRolesAsync()
{
    return await _context.Users
        .Include(u => u.Role)
        .OrderBy(u => u.Username)
        .ToListAsync();
}


            public async Task<User?> GetByIdAsync(int id)
            {
                return await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }

            public async Task UpdateAsync(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var user = await GetByIdAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
    }
    
}