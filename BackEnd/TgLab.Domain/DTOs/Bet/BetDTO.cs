﻿using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Bet
{
    public class BetDTO
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Stage { get; set; }
        public decimal Bounty { get; set; }
        public DateTime Time { get; set; }
    }
}
