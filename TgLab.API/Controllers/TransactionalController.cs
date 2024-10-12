using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TgLab.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Transactions")]
    public class TransactionalController : Controller
    {
        private readonly ILogger<TransactionalController> _logger;

        public TransactionalController(ILogger<TransactionalController> logger)
        {
            _logger = logger;
        }

        [HttpGet("List")]
        public IEnumerable<string> GetTransactions() 
        {
            throw new NotImplementedException();
        }
    }
}
