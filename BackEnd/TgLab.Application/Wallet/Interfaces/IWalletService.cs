using TgLab.Application.Wallet.DTOs;

namespace TgLab.Application.Wallet.Interfaces
{
    public interface IWalletService
    {
        public Task Create(CreateWalletDTO dto);
        public Task DecreaseBalance(int Id, decimal amount);
        public Task IncreaseBalance(int Id, decimal bounty);
    }
}
