﻿using TgLab.Application.Bet.DTOs;
using WalletDb = TgLab.Domain.Models.Wallet;

namespace TgLab.Application.Bet.Interfaces
{
    public interface IBetService
    {
        public Task Create(CreateGambleDTO dto, string userEmail);
        public bool InvalidBet(CreateGambleDTO bet, WalletDb wallet);
    }
}
