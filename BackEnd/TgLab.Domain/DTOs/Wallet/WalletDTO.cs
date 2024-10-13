using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Wallet
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
    }
}
