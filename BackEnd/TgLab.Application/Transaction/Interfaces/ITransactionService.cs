﻿using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Transaction;
using TgLab.Domain.Enums;
using BetDb = TgLab.Domain.Models.Bet;

namespace TgLab.Application.Transaction.Interfaces
{
    public interface ITransactionService
    {
        public Task Create(BetDb bet, TransactionType type);
        public Task<PaginatedList<TransactionDTO>> ListAll(string userEmail, int pageIndex, int pageSize);
        public Task<PaginatedList<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize);
    }
}
