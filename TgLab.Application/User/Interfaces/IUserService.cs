using TgLab.Application.User.DTOs;

namespace TgLab.Application.User.Interfaces
{
    public interface IUserService
    {
        public Task Create(CreateUserDTO dto);
    }
}
