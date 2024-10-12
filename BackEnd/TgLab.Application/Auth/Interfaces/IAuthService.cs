using TgLab.Domain.Auth;

namespace TgLab.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<LoggedUserDTO> Login(LoginDTO dto);
    }
}
