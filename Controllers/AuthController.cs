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

        [HttpPost("roles")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)] 
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.CreateRoleAsync(roleDto);
            if (!result)
            {
                return Conflict($"El rol '{roleDto.Name}' ya existe.");
            }

            return StatusCode(201, $"Rol '{roleDto.Name}' creado exitosamente.");
        }
    
    }
}
