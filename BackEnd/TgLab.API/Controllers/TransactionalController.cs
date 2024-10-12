using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TgLab.Application.Transaction.Interfaces;
using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Transaction;

namespace TgLab.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Transactions")]
    public class TransactionalController : Controller
    {
        private readonly ILogger<TransactionalController> _logger;
        private readonly ITransactionService _service;

        public TransactionalController(ILogger<TransactionalController> logger, ITransactionService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("List/{walletId}")]
        public async Task<ActionResult<ApiResponse>> ListTransactionsByWalletId(int walletId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _service.ListTransactionsByWalletId(walletId, userEmail, pageIndex, pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(ListTransactionsByWalletId)}] Invalid request: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(ListTransactionsByWalletId)}] Error trying to transactions by wallet: {ex.Message}", ex);
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
                _logger.LogError($"[{nameof(ListAll)}] Error trying to list transactions: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
