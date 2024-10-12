using TgLab.Domain.DTOs.Wallet;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Infrastructure.Context;
using WalletDb = TgLab.Domain.Models.Wallet;

namespace TgLab.Application.Wallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly TgLabContext _context;

        public WalletService(TgLabContext context)
        {
            _context = context;
        }

        public async Task Create(CreateWalletDTO dto, string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = new WalletDb()
            {
                UserId = user.Id,
                Currency = dto.Currency,
                Balance = dto.Balance,
            };

            _context.Wallets.Add(wallet);
            _context.SaveChanges();
        }

        public Task DecreaseBalance(int Id, decimal amount)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Id == Id);

            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance -= amount;

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task IncreaseBalance(int Id, decimal bounty)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Id == Id);

            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance += bounty;

            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
