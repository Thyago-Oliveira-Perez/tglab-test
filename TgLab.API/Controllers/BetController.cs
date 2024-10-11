using Microsoft.AspNetCore.Mvc;

namespace TgLab.API.Controllers
{
    [ApiController]
    [Route("Bet")]
    public class BetController : Controller
    {
        private readonly ILogger<BetController> _logger;

        public BetController(ILogger<BetController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Gamble")]
        public bool DoBet()
        {
            throw new NotImplementedException();
        }

        [HttpPost("Cancel")]
        public bool CancellBet()
        {
            throw new NotImplementedException();
        }

        [HttpGet("List")]
        public IEnumerable<string> GetBets()
        {
            throw new NotImplementedException();
        }
    }
}
