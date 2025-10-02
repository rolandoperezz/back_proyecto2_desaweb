using desawebback.DTOs;
using desawebback.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace desawebback.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IAdminService _adminService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.AuthenticateAsync(request);
            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result == null)
                return BadRequest("El usuario ya se encuentra registro o el nombre del perfil no es correcto");

            return Ok(new { message = result });
        }

           // ====== ROLES ======
        [HttpPost("roles")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _authService.CreateRoleAsync(roleDto);
            if (!ok) return Conflict($"El rol '{roleDto.Name}' ya existe.");

            return StatusCode(201, $"Rol '{roleDto.Name}' creado exitosamente.");
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _authService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("roles/{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _authService.GetRoleByIdAsync(id);
            if (role == null) return NotFound("Rol no encontrado");
            return Ok(role);
        }

        [HttpPut("roles/{id:int}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _authService.UpdateRoleAsync(id, dto);
            if (!ok) return Conflict("Ya existe un rol con ese nombre o el rol no existe.");

            return Ok();
        }

        [HttpDelete("roles/{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var ok = await _authService.DeleteRoleAsync(id);
            if (!ok) return NotFound("Rol no encontrado");
            return Ok(new { message = "Rol eliminado correctamente" });
        }

        // ====== USERS ======
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null) return NotFound("Usuario no encontrado");
            return Ok(user);
        }

        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _authService.UpdateUserAsync(id, updateDto);
            if (!ok) return BadRequest("No se pudo actualizar el usuario. Verifica que el username no esté en uso y que el rol exista.");
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var ok = await _authService.DeleteUserAsync(id);
            if (!ok) return NotFound("Usuario no encontrado");
            return Ok(new { message = "Usuario eliminado correctamente" });
        }

        [HttpPut("users/{id:int}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _authService.ChangePasswordAsync(id, changePasswordDto);
            if (!ok) return NotFound("Usuario no encontrado");

            return Ok(new { message = "Contraseña actualizada correctamente" });
        }
    
    }
}
