using Microsoft.EntityFrameworkCore;
using TgLab.Application.Transactional.DTOs;
using TgLab.Application.Transactional.Interfaces;
using TgLab.Application.User.Interfaces;
using TgLab.Infrastructure.Context;

namespace TgLab.Application.Transactional.Services
{
    public class TransactionalService : ITransactionalService
    {
        private readonly TgLabContext _context;
        private readonly IUserService _userService;

        public TransactionalService(TgLabContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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
