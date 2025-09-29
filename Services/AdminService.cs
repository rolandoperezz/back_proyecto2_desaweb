using desawebback.DTOs;
using desawebback.Models;
using desawebback.Repositories;
using desawebback.Repositories.Interfaces;
using desawebback.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace desawebback.Services
{
    public class AdminService : IAdminService
    {
        private readonly IEquipoRepository _equipoRepository;
                private readonly IRoleRepository _roleRepository;

        private readonly IJugadorRepository _jugadorRepository;
        private readonly IPartidoRepository _partidoRepository;

        public AdminService(IEquipoRepository equipoRepository, IJugadorRepository jugadorRepository, IPartidoRepository partidoRepository)
        {
            _equipoRepository = equipoRepository;
            _jugadorRepository = jugadorRepository;
            _partidoRepository = partidoRepository;
        }


        public async Task<IEnumerable<EquipoResponseDto>> GetEquiposAsync(string searchTerm = null, string cityFilter = null)
        {
            var equipos = await _equipoRepository.GetAllAsync(searchTerm, cityFilter);
            return equipos.Select(e => new EquipoResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Ciudad = e.Ciudad,
                LogoUrl = e.LogoUrl
            });
        }

        public async Task<EquipoResponseDto> GetEquipoByIdAsync(int id)
        {
            var equipo = await _equipoRepository.GetByIdAsync(id);
            if (equipo == null) return null;
            return new EquipoResponseDto
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                Ciudad = equipo.Ciudad,
                LogoUrl = equipo.LogoUrl
            };
        }

        public async Task<EquipoResponseDto> CreateEquipoAsync(EquipoDto equipoDto)
        {
            var nuevoEquipo = new Equipo
            {
                Nombre = equipoDto.Nombre,
                Ciudad = equipoDto.Ciudad,
                LogoUrl = equipoDto.LogoUrl
            };
            await _equipoRepository.AddAsync(nuevoEquipo);
            return new EquipoResponseDto
            {
                Id = nuevoEquipo.Id,
                Nombre = nuevoEquipo.Nombre,
                Ciudad = nuevoEquipo.Ciudad,
                LogoUrl = nuevoEquipo.LogoUrl
            };
        }

        public async Task<bool> UpdateEquipoAsync(int id, EquipoDto equipoDto)
        {
            var equipoExistente = await _equipoRepository.GetByIdAsync(id);
            if (equipoExistente == null) return false;

            equipoExistente.Nombre = equipoDto.Nombre;
            equipoExistente.Ciudad = equipoDto.Ciudad;
            equipoExistente.LogoUrl = equipoDto.LogoUrl;

            await _equipoRepository.UpdateAsync(equipoExistente);
            return true;
        }

        public async Task<bool> DeleteEquipoAsync(int id)
        {
            var equipoExistente = await _equipoRepository.GetByIdAsync(id);
            if (equipoExistente == null) return false;

            await _equipoRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<JugadorResponseDto>> GetJugadoresByEquipoAsync(int equipoId, string searchTerm = null, string positionFilter = null)
        {
            if (!await _jugadorRepository.EquipoExistsAsync(equipoId)) return null; // Equipo no encontrado

            var jugadores = await _jugadorRepository.GetAllByEquipoAsync(equipoId, searchTerm, positionFilter);
            return jugadores.Select(j => new JugadorResponseDto
            {
                Id = j.Id,
                NombreCompleto = j.NombreCompleto,
                Numero = j.Numero,
                Posicion = j.Posicion,
                EstaturaCm = j.EstaturaCm,
                Edad = j.Edad,
                Nacionalidad = j.Nacionalidad,
                Equipo = new EquipoResponseDto { Id = j.Equipo.Id, Nombre = j.Equipo.Nombre, LogoUrl = j.Equipo.LogoUrl } // Asumiendo que el equipo viene cargado
            });
        }

        public async Task<JugadorResponseDto> GetJugadorByIdAsync(int id)
        {
            var jugador = await _jugadorRepository.GetByIdAsync(id);
            if (jugador == null) return null;
            return new JugadorResponseDto
            {
                Id = jugador.Id,
                NombreCompleto = jugador.NombreCompleto,
                Numero = jugador.Numero,
                Posicion = jugador.Posicion,
                EstaturaCm = jugador.EstaturaCm,
                Edad = jugador.Edad,
                Nacionalidad = jugador.Nacionalidad,
                Equipo = new EquipoResponseDto { Id = jugador.Equipo.Id, Nombre = jugador.Equipo.Nombre, LogoUrl = jugador.Equipo.LogoUrl}
            };
        }

        public async Task<JugadorResponseDto> CreateJugadorAsync(JugadorDto jugadorDto)
        {
            if (!await _jugadorRepository.EquipoExistsAsync(jugadorDto.EquipoId)) return null; // Equipo no encontrado

            var nuevoJugador = new Jugador
            {
                NombreCompleto = jugadorDto.NombreCompleto,
                Numero = jugadorDto.Numero,
                Posicion = jugadorDto.Posicion,
                EstaturaCm = jugadorDto.EstaturaCm,
                Edad = jugadorDto.Edad,
                Nacionalidad = jugadorDto.Nacionalidad,
                EquipoId = jugadorDto.EquipoId
            };
            await _jugadorRepository.AddAsync(nuevoJugador);

            var jugadorCreado = await _jugadorRepository.GetByIdAsync(nuevoJugador.Id);
            return new JugadorResponseDto
            {
                Id = jugadorCreado.Id,
                NombreCompleto = jugadorCreado.NombreCompleto,
                Numero = jugadorCreado.Numero,
                Posicion = jugadorCreado.Posicion,
                EstaturaCm = jugadorCreado.EstaturaCm,
                Edad = jugadorCreado.Edad,
                Nacionalidad = jugadorCreado.Nacionalidad,
                Equipo = new EquipoResponseDto { Id = jugadorCreado.Equipo.Id, Nombre = jugadorCreado.Equipo.Nombre, LogoUrl = jugadorCreado.Equipo.LogoUrl }
            };
        }

        public async Task<bool> UpdateJugadorAsync(int id, JugadorDto jugadorDto)
        {
            var jugadorExistente = await _jugadorRepository.GetByIdAsync(id);
            if (jugadorExistente == null) return false;
            if (!await _jugadorRepository.EquipoExistsAsync(jugadorDto.EquipoId)) return false; // Nuevo equipo no encontrado

            jugadorExistente.NombreCompleto = jugadorDto.NombreCompleto;
            jugadorExistente.Numero = jugadorDto.Numero;
            jugadorExistente.Posicion = jugadorDto.Posicion;
            jugadorExistente.EstaturaCm = jugadorDto.EstaturaCm;
            jugadorExistente.Edad = jugadorDto.Edad;
            jugadorExistente.Nacionalidad = jugadorDto.Nacionalidad;
            jugadorExistente.EquipoId = jugadorDto.EquipoId;

            await _jugadorRepository.UpdateAsync(jugadorExistente);
            return true;
        }

        public async Task<bool> DeleteJugadorAsync(int id)
        {
            var jugadorExistente = await _jugadorRepository.GetByIdAsync(id);
            if (jugadorExistente == null) return false;

            await _jugadorRepository.DeleteAsync(id);
            return true;
        }

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
                                    .Select(jp => new JugadorResponseDto
                                    {
                                        Id = jp.Jugador.Id,
                                        NombreCompleto = jp.Jugador.NombreCompleto,
                                        Numero = jp.Jugador.Numero,
                                        Posicion = jp.Jugador.Posicion,
                                        Equipo = new EquipoResponseDto { Id = jp.Jugador.Equipo.Id, Nombre = jp.Jugador.Equipo.Nombre }
                                    }).ToList();

            var rosterVisitante = partido.Roster
                                        .Where(jp => jp.Jugador.EquipoId == partido.EquipoVisitanteId)
                                        .Select(jp => new JugadorResponseDto
                                        {
                                            Id = jp.Jugador.Id,
                                            NombreCompleto = jp.Jugador.NombreCompleto,
                                            Numero = jp.Jugador.Numero,
                                            Posicion = jp.Jugador.Posicion,
                                            Equipo = new EquipoResponseDto { Id = jp.Jugador.Equipo.Id, Nombre = jp.Jugador.Equipo.Nombre }
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

        public async Task<PartidoResponseDto> CreatePartidoAsync(CrearPartidoDto partidoDto)
        {
            if (!await _partidoRepository.EquipoExistsAsync(partidoDto.EquipoLocalId) ||
                !await _partidoRepository.EquipoExistsAsync(partidoDto.EquipoVisitanteId) ||
                partidoDto.EquipoLocalId == partidoDto.EquipoVisitanteId)
            {
                return null; 
            }

            var nuevoPartido = new Partido
            {
                FechaHora = partidoDto.FechaHora,
                EquipoLocalId = partidoDto.EquipoLocalId,
                EquipoVisitanteId = partidoDto.EquipoVisitanteId
            };
            await _partidoRepository.AddAsync(nuevoPartido);

            var partidoCreado = await _partidoRepository.GetByIdAsync(nuevoPartido.Id);
            return new PartidoResponseDto
            {
                Id = partidoCreado.Id,
                FechaHora = partidoCreado.FechaHora,
                MarcadorLocal = partidoCreado.MarcadorLocal,
                MarcadorVisitante = partidoCreado.MarcadorVisitante,
                EquipoLocal = new EquipoResponseDto { Id = partidoCreado.EquipoLocal.Id, Nombre = partidoCreado.EquipoLocal.Nombre, LogoUrl = partidoCreado.EquipoLocal.LogoUrl },
                EquipoVisitante = new EquipoResponseDto { Id = partidoCreado.EquipoVisitante.Id, Nombre = partidoCreado.EquipoVisitante.Nombre, LogoUrl = partidoCreado.EquipoVisitante.LogoUrl }
            };
        }

        public async Task<bool> UpdatePartidoAsync(int id, CrearPartidoDto partidoDto)
        {
            var partidoExistente = await _partidoRepository.GetByIdAsync(id);
            if (partidoExistente == null) return false;

            if (!await _partidoRepository.EquipoExistsAsync(partidoDto.EquipoLocalId) ||
                !await _partidoRepository.EquipoExistsAsync(partidoDto.EquipoVisitanteId) ||
                partidoDto.EquipoLocalId == partidoDto.EquipoVisitanteId)
            {
                return false; 
            }

            partidoExistente.FechaHora = partidoDto.FechaHora;
            partidoExistente.EquipoLocalId = partidoDto.EquipoLocalId;
            partidoExistente.EquipoVisitanteId = partidoDto.EquipoVisitanteId;

            await _partidoRepository.UpdateAsync(partidoExistente);
            return true;
        }

        public async Task<bool> DeletePartidoAsync(int id)
        {
            var partidoExistente = await _partidoRepository.GetByIdAsync(id);
            if (partidoExistente == null) return false;

            await _partidoRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AssignRosterToPartidoAsync(int partidoId, int equipoId, List<int> jugadorIds)
        {
            var partido = await _partidoRepository.GetByIdAsync(partidoId);
            if (partido == null) return false;

            if (partido.EquipoLocalId != equipoId && partido.EquipoVisitanteId != equipoId)
            {
                return false; 
            }

            await _partidoRepository.AddPlayersToRosterAsync(partidoId, equipoId, jugadorIds);
            return true;
        }

        public async Task<bool> UpdatePartidoMarcadorAsync(int partidoId, ActualizarMarcadorPartidoDto marcadorDto)
        {
            if (!await _partidoRepository.ExistsAsync(partidoId)) return false;
            await _partidoRepository.UpdateScoreAsync(partidoId, marcadorDto.MarcadorLocal, marcadorDto.MarcadorVisitante);
            return true;
        }
        
        
    }
}