using Microsoft.AspNetCore.Mvc;
using TgLab.Application.Auth.DTOs;
using TgLab.Application.Auth.Interfaces;

namespace TgLab.API.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var loggedUser = await _authService.Login(dto);
                return Ok(loggedUser);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"[{nameof(Login)}] User not found.", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Login)}] Error trying to login.", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
