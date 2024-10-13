using TgLab.Domain.DTOs.User;
using UserDb = TgLab.Domain.Models.User;

namespace TgLab.Domain.Interfaces.User
{
    public interface IUserService
    {
        public Task Create(CreateUserDTO dto);
        public Task<UserDb?> GetUserAndWalletsByEmail(string email);
    }
}
