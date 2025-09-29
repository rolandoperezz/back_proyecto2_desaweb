using desawebback.DTOs;
using desawebback.Models;
using desawebback.Repositories.Interfaces;
using desawebback.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desawebback.Services
{
    public class PartidoService(IPartidoRepository partidoRepository) : IPartidoService
    {
        private readonly IPartidoRepository _partidoRepository = partidoRepository;

        public async Task<IEnumerable<PartidoResponseDto>> GetPartidosAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var partidos = await _partidoRepository.GetAllAsync(startDate, endDate);
            return partidos.Select(p => new PartidoResponseDto
            {
                Id = p.Id,
                FechaHora = p.FechaHora,
                MarcadorLocal = p.MarcadorLocal,
                MarcadorVisitante = p.MarcadorVisitante,
                EquipoLocal = new EquipoResponseDto { Id = p.EquipoLocal.Id, Nombre = p.EquipoLocal.Nombre, LogoUrl = p.EquipoLocal.LogoUrl },
                EquipoVisitante = new EquipoResponseDto { Id = p.EquipoVisitante.Id, Nombre = p.EquipoVisitante.Nombre, LogoUrl = p.EquipoVisitante.LogoUrl }
            });
        }

        public async Task<PartidoResponseDto> GetPartidoByIdAsync(int id)
        {
            var partido = await _partidoRepository.GetByIdAsync(id);
            if (partido == null) return null;

            var rosterLocal = partido.Roster
                                     .Where(jp => jp.Jugador.EquipoId == partido.EquipoLocalId)
                                     .Select(jp => new JugadorResponseDto {
                                         Id = jp.Jugador.Id,
                                         NombreCompleto = jp.Jugador.NombreCompleto
                                     }).ToList();

            var rosterVisitante = partido.Roster
                                        .Where(jp => jp.Jugador.EquipoId == partido.EquipoVisitanteId)
                                        .Select(jp => new JugadorResponseDto {
                                            Id = jp.Jugador.Id,
                                            NombreCompleto = jp.Jugador.NombreCompleto
                                        }).ToList();

            return new PartidoResponseDto
            {
                Id = partido.Id,
                FechaHora = partido.FechaHora,
                MarcadorLocal = partido.MarcadorLocal,
                MarcadorVisitante = partido.MarcadorVisitante,
                EquipoLocal = new EquipoResponseDto { Id = partido.EquipoLocal.Id, Nombre = partido.EquipoLocal.Nombre, LogoUrl = partido.EquipoLocal.LogoUrl },
                EquipoVisitante = new EquipoResponseDto { Id = partido.EquipoVisitante.Id, Nombre = partido.EquipoVisitante.Nombre, LogoUrl = partido.EquipoVisitante.LogoUrl },
                RosterLocal = rosterLocal,
                RosterVisitante = rosterVisitante
            };
        }

        public async Task<bool> UpdatePartidoMarcadorAsync(int partidoId, ActualizarMarcadorPartidoDto marcadorDto)
        {
            var partido = await _partidoRepository.GetByIdAsync(partidoId);
            if (partido == null) return false;
            
            await _partidoRepository.UpdateScoreAsync(partidoId, marcadorDto.MarcadorLocal, marcadorDto.MarcadorVisitante);
            return true;
        }
    }
}