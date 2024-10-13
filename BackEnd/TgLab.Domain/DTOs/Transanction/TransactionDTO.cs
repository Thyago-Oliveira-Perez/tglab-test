using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Transaction
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
    }
}
