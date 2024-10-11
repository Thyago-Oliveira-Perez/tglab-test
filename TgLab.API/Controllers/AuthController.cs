using Microsoft.AspNetCore.Mvc;

namespace TgLab.API.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Login")]
        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}
