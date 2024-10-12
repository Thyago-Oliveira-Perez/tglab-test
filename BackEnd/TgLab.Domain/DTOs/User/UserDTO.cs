using TgLab.Domain.DTOs.Wallet;

namespace TgLab.Domain.DTOs.User
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<WalletDTO> Wallets { get; set; }
    }
}
