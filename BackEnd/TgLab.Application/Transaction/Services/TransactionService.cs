using Microsoft.EntityFrameworkCore;
using TgLab.Domain.DTOs.Transaction;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using BetDb = TgLab.Domain.Models.Bet;
using TransactionDb = TgLab.Domain.Models.Transaction;
using TgLab.Domain.DTOs;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Domain.Interfaces.User;

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
            var bonus = CalcBonus(bet, type);

            var newTransaction = new TransactionDb()
            {
                WalletId = bet.WalletId,
                Time = DateTime.Now,
                Type = type.Value,
                Amount = bet.Amount + bonus,
            };

            _context.Add(newTransaction);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<PaginatedList<TransactionDTO>> ListAll(string userEmail, int pageIndex, int pageSize)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.UserId == user.Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var walletIds = user.Wallets.Select(w => w.Id);

            var transactions = _context.Transactions
                .Where(t => walletIds.Contains(t.WalletId))
                .AsNoTracking()
                .Select(t => new TransactionDTO()
                {
                    Id = t.Id,
                    Time = t.Time,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await _context.Transactions.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<TransactionDTO>(transactions, pageIndex, totalPages);
        }

        public async Task<PaginatedList<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize)
        {
            var user = await _userService.GetUserAndWalletsByEmail(userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.SingleOrDefault(w => w.Id == walletId);

            ArgumentNullException.ThrowIfNull(wallet);

            var transactions = _context.Transactions
                .Where(t => t.WalletId == walletId)
                .AsNoTracking()
                .Select(t => new TransactionDTO()
                {
                    Id = t.Id,
                    Time = t.Time,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await _context.Transactions.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<TransactionDTO>(transactions, pageIndex, totalPages);
        }

        public decimal CalcBonus(BetDb bet, TransactionType type)
        {
            if (type.Equals(TransactionType.LOSS.Value))
                return 0;

            var lastFiveTransanctions = _context.Transactions
                .Where(t => t.WalletId == bet.WalletId)
                .Take(5)
                .AsNoTracking()
                .ToList();

            var allLosses = lastFiveTransanctions.All(lf => lf.Type.Equals(TransactionType.LOSS.Value));

            if (!allLosses && lastFiveTransanctions.Count == 5)
            {
                var totalLost = lastFiveTransanctions.Sum(lf => lf.Amount);

                return totalLost * 0.1M;
            }
 
            return 0;
        }
    }
}
