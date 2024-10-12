using Microsoft.AspNetCore.Mvc;
using TgLab.Application.User.DTOs;
using TgLab.Application.User.Interfaces;

namespace TgLab.API.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;

        public UserController(ILogger<UserController> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("Create")]
        public IActionResult CreateUser([FromBody] CreateUserDTO dto)
        {
            try
            {
                var result = _service.Create(dto);

                if (result.IsCompleted)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("User creation failed.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(CreateUser)}] Invalid argument: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(CreateUser)}] Error trying to create user: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
