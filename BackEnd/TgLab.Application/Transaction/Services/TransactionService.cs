using Microsoft.EntityFrameworkCore;
using TgLab.Application.Transaction.DTOs;
using TgLab.Application.Transaction.Interfaces;
using TgLab.Application.User.Interfaces;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using BetDb = TgLab.Domain.Models.Bet;
using TransactionDb = TgLab.Domain.Models.Transaction;

namespace TgLab.Application.Transaction.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TgLabContext _context;
        private readonly IUserService _userService;

        public TransactionService(TgLabContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public Task Create(BetDb bet, TransactionType type)
        {
            var newTransaction = new TransactionDb()
            {
                WalletId = bet.WalletId,
                Time = DateTime.Now,
                Type = type,
                Amount = bet.Amount,
            };

            _context.Add(newTransaction);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<TransactionDTO>> ListAll(string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.UserId == user.Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var walletIds = user.Wallets.Select(w => w.Id);

            return _context.Transactions
                .Where(t => walletIds.Contains(t.WalletId))
                .AsNoTracking()
                .Select(t => new TransactionDTO()
                {
                    Id = t.Id,
                    Time = t.Time,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .ToList();
        }

        public async Task<IEnumerable<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.Id == walletId);

            ArgumentNullException.ThrowIfNull(wallet);
            return _context.Transactions
                .Where(t => t.WalletId == walletId)
                .AsNoTracking()
                .Select(t => new TransactionDTO()
                {
                    Id = t.Id,
                    Time = t.Time,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .ToList();
        }
    }
}
