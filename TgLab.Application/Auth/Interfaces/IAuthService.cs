using TgLab.Application.Auth.DTOs;

namespace TgLab.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<LoggedUserDTO> Login(LoginDTO dto);
    }
}
