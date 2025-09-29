using desawebback.Data;
using desawebback.Models;
using desawebback.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desawebback.Repositories
{
    public class EquipoRepository : IEquipoRepository
    {
        private readonly AppDbContext _context;

        public EquipoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipo>> GetAllAsync(string searchTerm = null, string cityFilter = null)
        {
            var query = _context.Equipos.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Nombre.Contains(searchTerm) || e.Ciudad.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(cityFilter))
            {
                query = query.Where(e => e.Ciudad == cityFilter);
            }

            return await query.ToListAsync();
        }

        public async Task<Equipo> GetByIdAsync(int id)
        {
            return await _context.Equipos.FindAsync(id);
        }

        public async Task AddAsync(Equipo equipo)
        {
            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Equipo equipo)
        {
            _context.Entry(equipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Equipos.AnyAsync(e => e.Id == id);
        }
    }
}