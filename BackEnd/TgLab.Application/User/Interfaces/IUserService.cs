using TgLab.Application.User.DTOs;
using UserDb = TgLab.Domain.Models.User;

namespace TgLab.Application.User.Interfaces
{
    public interface IUserService
    {
        public Task Create(CreateUserDTO dto);
        public Task<UserDb?> GetUserAndWalletsByEmail(string email);
        public Task DecreaseUserBalance(int Id, int walletId, int amount);
    }
}
