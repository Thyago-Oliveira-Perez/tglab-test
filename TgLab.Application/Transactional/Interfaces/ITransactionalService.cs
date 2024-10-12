using TgLab.Application.Transactional.DTOs;

namespace TgLab.Application.Transactional.Interfaces
{
    public interface ITransactionalService
    {
        public Task<IEnumerable<TransactionDTO>> ListTransactionsByWalletId(int walletId, string userEmail);
        public Task<IEnumerable<TransactionDTO>> ListAll(string userEmail);
    }
}
