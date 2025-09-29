using desawebback.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace desawebback.Services.Interfaces
{
    public interface IPartidoService
    {
        Task<IEnumerable<PartidoResponseDto>> GetPartidosAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PartidoResponseDto> GetPartidoByIdAsync(int id);

        Task<bool> UpdatePartidoMarcadorAsync(int partidoId, ActualizarMarcadorPartidoDto marcadorDto);
    }
}