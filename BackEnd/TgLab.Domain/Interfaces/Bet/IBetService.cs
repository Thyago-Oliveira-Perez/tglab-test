using TgLab.Domain.DTOs;
using TgLab.Domain.DTOs.Bet;
using WalletDb = TgLab.Domain.Models.Wallet;
using BetDb = TgLab.Domain.Models.Bet;

namespace TgLab.Domain.Interfaces.Bet
{
    public interface IBetService
    {
        public Task Create(CreateGambleDTO dto, string userEmail);
        public Task<PaginatedList<BetDTO>> ListBetsByWalletId(int walletId, string userEmail, int pageIndex, int pageSize);
        public Task<PaginatedList<BetDTO>> ListAll(string userEmail, int pageIndex, int pageSize);
        public Task Cancel(int id, string userEmail);
        public bool InvalidBet(CreateGambleDTO bet, WalletDb wallet);
        public void Update(BetDb bet);
        public bool IsCancelled(int id);
    }
}
