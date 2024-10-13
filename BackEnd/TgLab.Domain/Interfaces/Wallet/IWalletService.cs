using TgLab.Domain.DTOs.Wallet;

namespace TgLab.Domain.Interfaces.Wallet
{
    public interface IWalletService
    {
        public Task Create(CreateWalletDTO dto, string userEmail);
        public Task DecreaseBalance(int Id, decimal amount);
        public Task IncreaseBalance(int Id, decimal bounty);
    }
}
