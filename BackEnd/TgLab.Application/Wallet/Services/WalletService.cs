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

        public async Task DecreaseBalance(int Id, decimal amount)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Id == Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var balance = wallet.Balance -= amount;

            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{balance}");
        }

        public async Task IncreaseBalance(int Id, decimal bounty)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Id == Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var balance = wallet.Balance += bounty;

            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{balance}");
        }
    }
}
