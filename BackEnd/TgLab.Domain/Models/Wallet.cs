using System.ComponentModel.DataAnnotations.Schema;

namespace TgLab.Domain.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }

        public virtual User User { get; set; }
        public virtual IEnumerable<Transaction> Transactions { get; set; }
        public virtual IEnumerable<Bet> Bets { get; set; }
    }
}
