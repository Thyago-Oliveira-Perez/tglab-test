using TgLab.Application.Transaction.DTOs;
using TgLab.Domain.Enums;
using BetDb = TgLab.Domain.Models.Bet;

namespace TgLab.Application.Transaction.Interfaces
{
    public interface ITransactionService
    {
        public Task Create(BetDb bet, TransactionType type);
        public Task<IEnumerable<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail);
        public Task<IEnumerable<TransactionDTO>> ListAll(string userEmail);
    }
}
