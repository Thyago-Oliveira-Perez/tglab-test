using TgLab.Domain.Enums;

namespace TgLab.Application.Bet.DTOs
{
    public class CreateGambleDTO
    {
        public int WalletId { get; set; }
        public int Amount { get; set; }
    }
}
