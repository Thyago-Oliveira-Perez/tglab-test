using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TgLab.Application.Bet.DTOs;
using TgLab.Application.Bet.Interfaces;

namespace TgLab.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Bet")]
    public class BetController : Controller
    {
        private readonly ILogger<BetController> _logger;
        private readonly IBetService _service;

        public BetController(ILogger<BetController> logger, IBetService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("Gamble")]
        public async Task<IActionResult> Gamble([FromBody] CreateGambleDTO dto)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _service.Create(dto, userEmail);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(Gamble)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Gamble)}] Error trying to gamble: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("Cancel")]
        public bool Cancel()
        {
            throw new NotImplementedException();
        }

        [HttpGet("List/{walletId}")]
        public async Task<ActionResult<IEnumerable<BetDTO>>> ListBetsByWalletId(int walletId)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _service.ListBetsByWalletId(walletId, userEmail);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(ListBetsByWalletId)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ListBetsByWalletId)}] Error trying to list bets: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
