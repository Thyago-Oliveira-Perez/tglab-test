using TgLab.Domain.DTOs.Wallet;
using TgLab.Infrastructure.Context;
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.Interfaces.Notification;
using TgLab.Domain.Interfaces.Wallet;

namespace TgLab.Application.Wallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly TgLabContext _context;
        private readonly INotificationService _notificationService;

        public WalletService(TgLabContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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

        public async Task DecreaseBalance(WalletDb wallet, decimal amount)
        {
            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance -= amount;

            _context.Wallets.Update(wallet);
            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{wallet.Balance}");
        }

        public async Task IncreaseBalance(WalletDb wallet, decimal bounty)
        {
            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance += bounty;

            _context.Wallets.Update(wallet);
            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{wallet.Balance}");
        }
    }
}
