using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.DTOs;
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

        public Task Create(CreateWalletDTO dto)
        {
            var wallet = new WalletDb()
            {
                UserId = dto.UserId,
                Currency = dto.Currency,
                Balance = dto.Balance,
            };

            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
