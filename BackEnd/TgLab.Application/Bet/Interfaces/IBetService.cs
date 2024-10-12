using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Bet;
using WalletDb = TgLab.Domain.Models.Wallet;

namespace TgLab.Application.Bet.Interfaces
{
    public interface IBetService
    {
        public Task Create(CreateGambleDTO dto, string userEmail);
        public Task<PaginatedList<BetDTO>> ListBetsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize);
        public Task<PaginatedList<BetDTO>> ListAll(string userEmail, int pageIndex, int pageSize);
        public Task Cancel(int id, string userEmail);
        public bool InvalidBet(CreateGambleDTO bet, WalletDb wallet);
    }
}
