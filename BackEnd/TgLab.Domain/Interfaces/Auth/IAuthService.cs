using TgLab.Domain.Auth;

namespace TgLab.Domain.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoggedUserDTO> Login(LoginDTO dto);
    }
}
