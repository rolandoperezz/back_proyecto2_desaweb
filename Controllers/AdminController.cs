using desawebback.DTOs;
using desawebback.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

       
        [HttpPost("equipos")]
        [ProducesResponseType(201, Type = typeof(EquipoResponseDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEquipo([FromBody] EquipoDto equipoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var equipo = await _adminService.CreateEquipoAsync(equipoDto);
            return CreatedAtAction(nameof(GetEquipoById), new { id = equipo.Id }, equipo);
        }

        [HttpPut("equipos/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEquipo(int id, [FromBody] EquipoDto equipoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.UpdateEquipoAsync(id, equipoDto);
            if (!result)
            {
                return NotFound($"Equipo con ID {id} no encontrado.");
            }
            return Ok($"Equipo con ID {id} actualizado exitosamente.");
        }

        [HttpDelete("equipos/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEquipo(int id)
        {
            var result = await _adminService.DeleteEquipoAsync(id);
            if (!result)
            {
                return NotFound($"Equipo con ID {id} no encontrado.");
            }
            return NoContent(); 
        }

        [HttpGet("equipos")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<EquipoResponseDto>))]
            public async Task<IActionResult> GetEquipos(
                [FromQuery] string? searchTerm = null,
                [FromQuery] string? cityFilter = null)
            {
                var equipos = await _adminService.GetEquiposAsync(searchTerm, cityFilter);
                return Ok(equipos);
            }

        [HttpGet("equipos/{id}")]
        [ProducesResponseType(200, Type = typeof(EquipoResponseDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEquipoById(int id)
        {
            var equipo = await _adminService.GetEquipoByIdAsync(id);
            if (equipo == null)
            {
                return NotFound($"Equipo con ID {id} no encontrado.");
            }
            return Ok(equipo);
        }

     
        [HttpPost("jugadores")]
        [ProducesResponseType(201, Type = typeof(JugadorResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateJugador([FromBody] JugadorDto jugadorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var jugador = await _adminService.CreateJugadorAsync(jugadorDto);
            if (jugador == null)
            {
                return NotFound($"Equipo con ID {jugadorDto.EquipoId} no encontrado.");
            }
            return CreatedAtAction(nameof(GetJugadorById), new { id = jugador.Id }, jugador);
        }

        [HttpPut("jugadores/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateJugador(int id, [FromBody] JugadorDto jugadorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.UpdateJugadorAsync(id, jugadorDto);
            if (!result)
            {
                return NotFound($"Jugador con ID {id} no encontrado o el Equipo ID {jugadorDto.EquipoId} no existe.");
            }
            return Ok($"Jugador con ID {id} actualizado exitosamente.");
        }

        [HttpDelete("jugadores/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteJugador(int id)
        {
            var result = await _adminService.DeleteJugadorAsync(id);
            if (!result)
            {
                return NotFound($"Jugador con ID {id} no encontrado.");
            }
            return NoContent();
        }

        [HttpGet("equipos/{equipoId}/jugadores")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<JugadorResponseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetJugadoresByEquipo(
            int equipoId,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? positionFilter = null)
        {
            var jugadores = await _adminService.GetJugadoresByEquipoAsync(equipoId, searchTerm, positionFilter);
            if (jugadores == null)
            {
                return NotFound($"Equipo con ID {equipoId} no encontrado.");
            }
            return Ok(jugadores);
        }


        [HttpGet("jugadores/{id}")]
        [ProducesResponseType(200, Type = typeof(JugadorResponseDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetJugadorById(int id)
        {
            var jugador = await _adminService.GetJugadorByIdAsync(id);
            if (jugador == null)
            {
                return NotFound($"Jugador con ID {id} no encontrado.");
            }
            return Ok(jugador);
        }

       
        [HttpPost("partidos")]
        [ProducesResponseType(201, Type = typeof(PartidoResponseDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePartido([FromBody] CrearPartidoDto partidoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var partido = await _adminService.CreatePartidoAsync(partidoDto);
            if (partido == null)
            {
                return BadRequest("Equipos no válidos o son el mismo equipo.");
            }
            return CreatedAtAction(nameof(GetPartidoById), new { id = partido.Id }, partido);
        }

        [HttpPut("partidos/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePartido(int id, [FromBody] CrearPartidoDto partidoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.UpdatePartidoAsync(id, partidoDto);
            if (!result)
            {
                return NotFound($"Partido con ID {id} no encontrado o los equipos no son válidos.");
            }
            return Ok($"Partido con ID {id} actualizado exitosamente.");
        }

        [HttpDelete("partidos/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePartido(int id)
        {
            var result = await _adminService.DeletePartidoAsync(id);
            if (!result)
            {
                return NotFound($"Partido con ID {id} no encontrado.");
            }
            return NoContent();
        }


        [HttpPost("partidos/{partidoId}/roster/equipo/{equipoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AssignRoster(int partidoId, int equipoId, [FromBody] List<int> jugadorIds)
        {
            if (jugadorIds == null || jugadorIds.Count == 0)
            {
                return BadRequest("Se deben proporcionar IDs de jugadores para el roster.");
            }

            var result = await _adminService.AssignRosterToPartidoAsync(partidoId, equipoId, jugadorIds);
            if (!result)
            {
                return NotFound($"Partido con ID {partidoId} no encontrado o equipo con ID {equipoId} no es parte del partido, o jugadores inválidos.");
            }
            return Ok($"Roster asignado exitosamente al partido {partidoId} para el equipo {equipoId}.");
        }

        [HttpGet("partidos")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PartidoResponseDto>))]
        public async Task<IActionResult> GetPartidos([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var partidos = await _adminService.GetPartidosAsync(startDate, endDate);
            return Ok(partidos);
        }

        [HttpGet("partidos/{id}")]
        [ProducesResponseType(200, Type = typeof(PartidoResponseDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPartidoById(int id)
        {
            var partido = await _adminService.GetPartidoByIdAsync(id);
            if (partido == null)
            {
                return NotFound($"Partido con ID {id} no encontrado.");
            }
            return Ok(partido);
        }

        [HttpPut("partidos/{id}/marcador")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMarcador(int id, [FromBody] ActualizarMarcadorPartidoDto marcadorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminService.UpdatePartidoMarcadorAsync(id, marcadorDto);
            if (!result)
            {
                return NotFound($"Partido con ID {id} no encontrado.");
            }
            return Ok($"Marcador del partido {id} actualizado exitosamente.");
        }


      
    }
}