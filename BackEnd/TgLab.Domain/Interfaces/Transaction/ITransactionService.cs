using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Transaction;
using TgLab.Domain.DTOs.Transanction;

namespace TgLab.Domain.Interfaces.Transaction
{
    public interface ITransactionService
    {
        public Task Create(CreateTransactionDTO dto);
        public Task<PaginatedList<TransactionDTO>> ListAll(string userEmail, int pageIndex, int pageSize);
        public Task<PaginatedList<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize);
        public decimal CalcBonus(int walletId, string type);
    }
}
