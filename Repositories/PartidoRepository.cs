using desawebback.Data;
using desawebback.Models;
using desawebback.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desawebback.Repositories
{
    public class PartidoRepository : IPartidoRepository
    {
        private readonly AppDbContext _context;

        public PartidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partido>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Partidos
                                .Include(p => p.EquipoLocal)
                                .Include(p => p.EquipoVisitante)
                                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(p => p.FechaHora >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.FechaHora <= endDate.Value);
            }

            return await query.OrderByDescending(p => p.FechaHora).ToListAsync();
        }

        public async Task<Partido> GetByIdAsync(int id)
        {
            return await _context.Partidos
                                 .Include(p => p.EquipoLocal)
                                 .Include(p => p.EquipoVisitante)
                                 .Include(p => p.Roster)
                                     .ThenInclude(jp => jp.Jugador)
                                         .ThenInclude(j => j.Equipo) 
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Partido partido)
        {
            await _context.Partidos.AddAsync(partido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Partido partido)
        {
            _context.Entry(partido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido != null)
            {
                _context.Partidos.Remove(partido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Partidos.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> EquipoExistsAsync(int equipoId)
        {
            return await _context.Equipos.AnyAsync(e => e.Id == equipoId);
        }

        public async Task AddPlayersToRosterAsync(int partidoId, int equipoId, List<int> jugadorIds)
        {
            var partido = await _context.Partidos.Include(p => p.Roster).FirstOrDefaultAsync(p => p.Id == partidoId);
            if (partido == null) return;

            var jugadoresValidos = await _context.Jugadores
                .Where(j => jugadorIds.Contains(j.Id) && j.EquipoId == equipoId)
                .ToListAsync();

            foreach (var jugador in jugadoresValidos)
            {
                if (!partido.Roster.Any(jp => jp.JugadorId == jugador.Id))
                {
                    partido.Roster.Add(new JugadorPartido { JugadorId = jugador.Id, PartidoId = partidoId });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Jugador>> GetRosterByPartidoAndEquipoAsync(int partidoId, int equipoId)
        {
             return await _context.JugadoresPartidos
                .Where(jp => jp.PartidoId == partidoId && jp.Jugador.EquipoId == equipoId)
                .Select(jp => jp.Jugador)
                .Include(j => j.Equipo)
                .ToListAsync();
        }

        public async Task UpdateScoreAsync(int partidoId, int marcadorLocal, int marcadorVisitante)
        {
            var partido = await _context.Partidos.FindAsync(partidoId);
            if (partido == null) return;

            partido.MarcadorLocal = marcadorLocal;
            partido.MarcadorVisitante = marcadorVisitante;
            _context.Entry(partido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}