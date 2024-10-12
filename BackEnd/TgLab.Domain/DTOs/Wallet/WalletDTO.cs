using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Wallet
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
