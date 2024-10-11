using TgLab.Domain.Enums;

namespace TgLab.Domain.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Balance { get; set; }
        public Currency Currency { get; set; }

        public virtual IEnumerable<Transactions> Transactions { get; set; }
        public virtual IEnumerable<Bet> Bets { get; set; }
    }
}
