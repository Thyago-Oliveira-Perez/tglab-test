using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TgLab.Domain.DTOs.Wallet;
using TgLab.Domain.Interfaces.Wallet;

namespace TgLab.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Wallet")]
    public class WalletController : Controller
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletService _service;

        public WalletController(ILogger<WalletController> logger, IWalletService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateWalletDTO dto)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = _service.Create(dto, userEmail);

                if (result.IsCompleted)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Wallet creation failed.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(Create)}] Invalid argument: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Create)}] Error trying to create wallet: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] DepositWalletedDTO dto)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = _service.Deposit(dto, userEmail);

                if (result.IsCompleted)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Wallet deposit failed.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(Deposit)}] Invalid argument: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Deposit)}] Error trying to deposit balance in wallet: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("Withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawWalletDTO dto)
        {
            try
            {
                string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = _service.Withdraw(dto, userEmail);

                if (result.IsCompleted)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Wallet withdraw failed.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"[{nameof(Withdraw)}] Invalid argument: {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(Withdraw)}] Error trying to withdraw balance: {ex.Message}", ex);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
