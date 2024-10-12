using TgLab.Application.Wallet.DTOs;

namespace TgLab.Application.User.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<WalletDTO> Wallets { get; set; }
    }
}
