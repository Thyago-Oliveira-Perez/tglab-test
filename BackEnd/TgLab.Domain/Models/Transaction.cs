using TgLab.Domain.Enums;

namespace TgLab.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public DateTime Time { get; set; }
        public TransactionType Type {get ; set;}
        public int Amount { get; set;}

        public virtual Wallet Wallet { get; set; }
    }
}
