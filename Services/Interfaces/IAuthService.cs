using desawebback.DTOs;

namespace desawebback.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
        Task<string?> RegisterAsync(RegisterRequestDto request);
        Task<bool> CreateRoleAsync(RoleDto roleDto);


    }
}
