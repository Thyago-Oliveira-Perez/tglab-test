using TgLab.Application.User.DTOs;
using TgLab.Infrastructure.Context;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Interfaces;
using UserDb = TgLab.Domain.Models.User;
using TgLab.Application.Wallet.DTOs;
using TgLab.Application.Wallet.Interfaces;

namespace TgLab.Application.User.Services
{
    public class UserService : IUserService
    {
        private readonly TgLabContext _context;
        private readonly IWalletService _walletService;
        public UserService(TgLabContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
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
                Password = dto.Password,
                BirthDate = dto.BirthDate,
            };

            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();

            var defaultWallet = new CreateWalletDTO().CreateDefaultWallet(addedUser.Entity.Id);

            _walletService.Create(defaultWallet);

            return Task.CompletedTask;
        }
    }
}
