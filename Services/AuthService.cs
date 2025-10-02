using desawebback.Models;
using desawebback.DTOs;
using desawebback.Repositories.Interfaces;
using desawebback.Services.Interfaces;
using desawebback.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace desawebback.Services
{
    public class AuthService(IUserRepository userRepository, IRoleRepository roleRepository,
        IConfiguration config, CryptoHelper cryptoHelper) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _config = config;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly CryptoHelper _cryptoHelper = cryptoHelper;

        public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserWithRoleAsync(request.Username, request.Password);
            if (user == null)
                return null;
            var token = GenerateJwtToken(user);
            string roleName = "";
            if (user.Role != null)
            {
                roleName = user.Role.Name;
            }
            RoleDto roleDto = new()
            {
                Name = roleName
            };
            return new LoginResponseDto
            {
                Username = user.Username,
                Role = roleDto,
                Token = token
            };
        }

        public async Task<string?> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                return null;
            var role = await _roleRepository.GetByNameAsync(request.Role.Name);
            if (role == null)
                return null;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                RoleId = role.Id
            };

            await _userRepository.AddAsync(user);
            return "Usuario registrado correctamente.";
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var jwtSecretKey = jwtSettings["key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string roleName = "";
            if (user.Role != null)
            {
                roleName = user.Role.Name;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("Id", _cryptoHelper.Encrypt(user.Id.ToString()))
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<bool> CreateRoleAsync(RoleDto roleDto)
        {
            // Verifica si el rol ya existe
            var roleExists = await _roleRepository.ExistsByNameAsync(roleDto.Name);
            if (roleExists)
            {
                return false; // No se puede crear un rol duplicado
            }

            var role = new Role { Name = roleDto.Name };
            await _roleRepository.AddAsync(role);
            return true;
        }
        // Agregar antes del último cierre } de la clase

public async Task<List<UserDto>> GetAllUsersAsync()
{
    var users = await _userRepository.GetAllWithRolesAsync();
    return users.Select(u => new UserDto
    {
        Id = u.Id,
        Username = u.Username,
        RoleName = u.Role?.Name ?? "",
        RoleId = u.RoleId,
        CreatedAt = u.CreatedAt
    }).ToList();
}

public async Task<UserDto?> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null) return null;

    return new UserDto
    {
        Id = user.Id,
        Username = user.Username,
        RoleName = user.Role?.Name ?? "",
        RoleId = user.RoleId,
        CreatedAt = user.CreatedAt
    };
}

public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null) return false;

    var usernameExists = await _userRepository.GetByUsernameAsync(updateDto.Username);
    if (usernameExists != null && usernameExists.Id != id)
        return false;

    var role = await _roleRepository.GetByIdAsync(updateDto.RoleId);
    if (role == null) return false;

    user.Username = updateDto.Username;
    user.RoleId = updateDto.RoleId;
    user.UpdatedAt = DateTime.Now;

    await _userRepository.UpdateAsync(user);
    return true;
}

public async Task<bool> DeleteUserAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null) return false;

    await _userRepository.DeleteAsync(id);
    return true;
}

public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto changePasswordDto)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null) return false;

    user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
    user.UpdatedAt = DateTime.Now;

    await _userRepository.UpdateAsync(user);
    return true;
}

public async Task<List<RoleDto>> GetAllRolesAsync()
{
    var roles = await _roleRepository.GetAllAsync();
    return roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }).ToList();
}

public async Task<RoleDto?> GetRoleByIdAsync(int id)
{
    var role = await _roleRepository.GetByIdAsync(id);
    if (role == null) return null;

    return new RoleDto { Id = role.Id, Name = role.Name };
}

public async Task<bool> UpdateRoleAsync(int id, UpdateRoleDto updateDto)
{
    var role = await _roleRepository.GetByIdAsync(id);
    if (role == null) return false;

    var roleExists = await _roleRepository.ExistsByNameAsync(updateDto.Name);
    if (roleExists)
    {
        var existingRole = await _roleRepository.GetByNameAsync(updateDto.Name);
        if (existingRole != null && existingRole.Id != id)
            return false;
    }

    role.Name = updateDto.Name;
    role.UpdatedAt = DateTime.Now;

    await _roleRepository.UpdateAsync(role);
    return true;
}

public async Task<bool> DeleteRoleAsync(int id)
{
    var role = await _roleRepository.GetByIdAsync(id);
    if (role == null) return false;

    await _roleRepository.DeleteAsync(id);
    return true;
}
    }
}