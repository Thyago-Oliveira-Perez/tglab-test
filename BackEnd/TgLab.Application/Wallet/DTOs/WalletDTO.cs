using TgLab.Domain.Enums;

namespace TgLab.Application.Wallet.DTOs
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
