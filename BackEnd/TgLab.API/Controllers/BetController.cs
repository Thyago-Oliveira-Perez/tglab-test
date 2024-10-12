using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TgLab.Application.Bet.Interfaces;
using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Bet;

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

        [HttpPost("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _service.Cancel(id, userEmail);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(Cancel)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Cancel)}] Error trying to cancel bet: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("List/{walletId}")]
        public async Task<ActionResult<ApiResponse>> ListBetsByWalletId(int walletId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _service.ListBetsByWalletId(walletId, userEmail, pageIndex, pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(ListBetsByWalletId)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ListBetsByWalletId)}] Error trying to list bets by wallet: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("List")]
        public async Task<ActionResult<ApiResponse>> ListAll(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _service.ListAll(userEmail, pageIndex, pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(ListAll)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ListAll)}] Error trying to list bets: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
