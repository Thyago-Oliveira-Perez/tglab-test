using TgLab.Domain.DTOs.User;   
using TgLab.Infrastructure.Context;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Interfaces;
using UserDb = TgLab.Domain.Models.User;
using TgLab.Domain.DTOs.Wallet;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TgLab.Application.User.Services
{
    public class UserService : IUserService
    {
        private readonly TgLabContext _context;
        private readonly IWalletService _walletService;
        private readonly ICryptService _cryptService;

        public UserService(TgLabContext context, IWalletService walletService, ICryptService cryptService)
        {
            _context = context;
            _walletService = walletService;
            _cryptService = cryptService;
        }

        public Task Create(CreateUserDTO dto)
        {
            if(dto.IsUnder18())
            {
                throw new Under18Exception();
            }

            var user = new UserDb()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = _cryptService.HashPassword(dto.Password),
                BirthDate = dto.BirthDate,
            };

            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();

            var defaultWallet = new CreateWalletDTO().CreateDefaultWallet(addedUser.Entity.Id);

            _walletService.Create(defaultWallet);

            return Task.CompletedTask;
        }

        public async Task<UserDb?> GetUserAndWalletsByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.Wallets)
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public Task DecreaseUserBalance(int Id, int walletId, int amount)
        {
            var user = _context.Users
                .Include (u => u.Wallets)
                .FirstOrDefault(u => u.Id == Id);

            ArgumentNullException.ThrowIfNull(user);

            var wallet = user.Wallets.FirstOrDefault(w => w.Id == walletId);

            ArgumentNullException.ThrowIfNull(wallet);

            wallet.Balance -= amount;

            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
