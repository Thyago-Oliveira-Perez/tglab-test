using System.ComponentModel.DataAnnotations.Schema;

namespace TgLab.Domain.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public string Stage { get; set; }
        [Column(TypeName = "money")]
        public decimal Bounty {  get; set; } 
        public DateTime Time { get; set; }

        public Wallet Wallet { get; set; }
    }
}
