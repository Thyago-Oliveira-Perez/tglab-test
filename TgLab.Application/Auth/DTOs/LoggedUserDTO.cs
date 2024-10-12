using TgLab.Application.User.DTOs;
using TgLab.Application.Wallet.DTOs;

namespace TgLab.Application.Auth.DTOs
{
    public class LoggedUserDTO
    {
        public LoggedUserDTO(UserDTO user, string token)
        {
            Name = user.Name;
            Email = user.Email;
            Wallets = user.Wallets;
            Token = token;
        }

        public LoggedUserDTO() { }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IEnumerable<WalletDTO> Wallets { get; set; }
    }
}
