using TgLab.Application.User.DTOs;
using TgLab.Infrastructure.Context;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Interfaces;
using UserDb = TgLab.Domain.Models.User;
using TgLab.Application.Wallet.DTOs;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Auth.Interfaces;


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
    }
}
