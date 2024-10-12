using TgLab.Domain.DTOs.User;
using TgLab.Domain.DTOs.Wallet;

namespace TgLab.Domain.Auth
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
