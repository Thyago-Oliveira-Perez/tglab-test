using TgLab.Domain.Enums;

namespace TgLab.Application.Transactional.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public TransactionType Type { get; set; }
        public int Amount { get; set; }
    }
}
