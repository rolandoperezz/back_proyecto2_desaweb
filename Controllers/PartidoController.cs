using desawebback.DTOs;
using desawebback.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class PartidosController(IPartidoService partidoService) : ControllerBase
    {
        private readonly IPartidoService _partidoService = partidoService;

        [HttpGet]
        public async Task<IActionResult> GetPartidos([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var partidos = await _partidoService.GetPartidosAsync(startDate, endDate);
            return Ok(partidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartidoById(int id)
        {
            var partido = await _partidoService.GetPartidoByIdAsync(id);
            if (partido == null) return NotFound();
            return Ok(partido);
        }

        [HttpPut("{id}/marcador")]
        public async Task<IActionResult> UpdateMarcador(int id, [FromBody] ActualizarMarcadorPartidoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _partidoService.UpdatePartidoMarcadorAsync(id, dto);
            if (!result) return NotFound();

            return Ok(new { message = "Marcador actualizado exitosamente." });
        }
    }
}