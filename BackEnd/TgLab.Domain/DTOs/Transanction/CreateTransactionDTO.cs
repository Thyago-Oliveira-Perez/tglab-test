using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Transanction
{
    public class CreateTransactionDTO
    {
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}
