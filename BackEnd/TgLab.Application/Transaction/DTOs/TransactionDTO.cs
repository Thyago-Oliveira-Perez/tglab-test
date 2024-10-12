using TgLab.Domain.Enums;

namespace TgLab.Application.Transaction.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
