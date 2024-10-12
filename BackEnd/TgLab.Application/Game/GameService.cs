using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using TgLab.Application.Bet.Services;
using TgLab.Application.Transaction.Interfaces;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Domain.Enums;
using BetDb = TgLab.Domain.Models.Bet;
using Microsoft.Extensions.DependencyInjection;

namespace TgLab.Application.Game
{
    public class GameService : BackgroundService
    {
        private readonly ILogger<BetService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly ConcurrentQueue<BetDb> _betsQueue = new();

        public GameService(ILogger<BetService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void DoBet(BetDb bet)
        {
            _betsQueue.Enqueue(bet);
            _logger.LogInformation($"[{nameof(DoBet)}] A bet has been placed by {bet.Wallet.User.Name}.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_betsQueue.TryDequeue(out var bet))
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);

                    using var scope = _scopeFactory.CreateScope();
                    var won = CheckBetResult();

                    var _transactionalService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
                    var _walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();

                    if (won)
                    {

                        _logger.LogInformation($"[{nameof(ExecuteAsync)}] User {bet.Wallet.User.Name} won the bet!");

                        var bounty = CalcBounty(bet.Amount);

                        _transactionalService.Create(bet, TransactionType.WIN);
                        _walletService.IncreaseBalance(bet.WalletId, bounty);
                    }
                    else
                    {
                        _logger.LogInformation($"[{nameof(ExecuteAsync)}] User {bet.Wallet.User.Name} lost the bet.");
                        _transactionalService.Create(bet, TransactionType.LOSS);
                    }
                }

                await Task.Delay(1500, stoppingToken);
            }
        }

        private static bool CheckBetResult()
        {
            return new Random().Next(2) == 0;
        }

        private static decimal CalcBounty(decimal amount)
        {
            return amount * 2;
        }
    }
}
