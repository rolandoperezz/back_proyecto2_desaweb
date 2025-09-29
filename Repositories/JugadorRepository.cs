using desawebback.Data;
using desawebback.Models;
using desawebback.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desawebback.Repositories
{
    public class JugadorRepository : IJugadorRepository
    {
        private readonly AppDbContext _context;

        public JugadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Jugador>> GetAllByEquipoAsync(int equipoId, string? searchTerm = null, string? positionFilter = null)
        {
            var query = _context.Jugadores
                                .Include(j => j.Equipo)
                                .Where(j => j.EquipoId == equipoId)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(j => j.NombreCompleto.Contains(searchTerm) || j.Posicion.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(positionFilter))
            {
                query = query.Where(j => j.Posicion == positionFilter);
            }

            return await query.ToListAsync();
        }

        public async Task<Jugador> GetByIdAsync(int id)
        {
            return await _context.Jugadores
                                 .Include(j => j.Equipo)
                                 .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task AddAsync(Jugador jugador)
        {
            await _context.Jugadores.AddAsync(jugador);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Jugador jugador)
        {
            _context.Entry(jugador).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador != null)
            {
                _context.Jugadores.Remove(jugador);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Jugadores.AnyAsync(j => j.Id == id);
        }

        public async Task<bool> EquipoExistsAsync(int equipoId)
        {
            return await _context.Equipos.AnyAsync(e => e.Id == equipoId);
        }
    }
}