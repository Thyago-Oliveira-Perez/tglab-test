using TgLab.Domain.Enums;

namespace TgLab.Application.Bet.DTOs
{
    public class BetDTO
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public int Amount { get; set; }
        public BetStage Stage { get; set; }
        public int Bounty { get; set; }
        public DateTime Time { get; set; }
    }
}
