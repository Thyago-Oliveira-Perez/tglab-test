using Microsoft.AspNetCore.Mvc;

namespace TgLab.API.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public bool CreateUser()
        {
            throw new NotImplementedException();
        }
    }
}
