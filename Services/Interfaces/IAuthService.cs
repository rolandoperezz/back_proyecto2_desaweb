using desawebback.DTOs;

namespace desawebback.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
        Task<string?> RegisterAsync(RegisterRequestDto request);

 // Usuarios
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto changePasswordDto);

        // Roles
        Task<bool> CreateRoleAsync(RoleDto roleDto);
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<bool> UpdateRoleAsync(int id, UpdateRoleDto updateDto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
