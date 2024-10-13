using TgLab.Domain.DTOs.Wallet;
using TgLab.Infrastructure.Context;
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.Interfaces.Notification;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Domain.DTOs.Transanction;
using TgLab.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace TgLab.Application.Wallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly TgLabContext _context;
        private readonly INotificationService _notificationService;
        private readonly ITransactionService _transactionService;

        public WalletService(TgLabContext context, INotificationService notificationService, ITransactionService transactionService)
        {
            _context = context;
            _notificationService = notificationService;
            _transactionService = transactionService;
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

        public async Task Deposit(DepositWalletedDTO dto, string userEmail)
        {
            var user = _context.Users
                .Include(u => u.Wallets)
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.FirstOrDefault(u => u.Id == dto.Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var transaction = new CreateTransactionDTO()
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.DEPOSIT
            };

            await IncreaseBalance(wallet, dto.Amount);
            await _transactionService.Create(transaction);
        }

        public async Task Withdraw(WithdrawWalletDTO dto, string userEmail)
        {
            var user = _context.Users
                .Include(u => u.Wallets)
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == userEmail);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.FirstOrDefault(u => u.Id == dto.Id);

            ArgumentNullException.ThrowIfNull(wallet);

            var transaction = new CreateTransactionDTO()
            {
                WalletId = wallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.WITHDRAWAL
            };

            await DecreaseBalance(wallet, dto.Amount);
            await _transactionService.Create(transaction);
        }

        public async Task DecreaseBalance(WalletDb wallet, decimal amount)
        {
            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance -= amount;

            _context.Wallets.Update(wallet);
            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{wallet.Balance}");
        }

        public async Task IncreaseBalance(WalletDb wallet, decimal amount)
        {
            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance += amount;

            _context.Wallets.Update(wallet);
            _context.SaveChanges();

            await _notificationService.SendMessageAsync($"{wallet.Balance}");
        }
    }
}
