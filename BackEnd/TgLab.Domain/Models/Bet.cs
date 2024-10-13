using TgLab.Domain.Enums;

namespace TgLab.Domain.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Stage { get; set; }
        public decimal Bounty {  get; set; } 
        public DateTime Time { get; set; }

        public Wallet Wallet { get; set; }
    }
}
