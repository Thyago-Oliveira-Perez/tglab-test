using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Transanction
{
    public class CreateTransactionDTO
    {
        public int WalletId { get; set; }
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}
