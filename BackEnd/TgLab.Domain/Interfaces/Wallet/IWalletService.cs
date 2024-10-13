using TgLab.Domain.DTOs.Wallet;
using WalletDb = TgLab.Domain.Models.Wallet;

namespace TgLab.Domain.Interfaces.Wallet
{
    public interface IWalletService
    {
        public Task Create(CreateWalletDTO dto, string userEmail);
        public Task Deposit(DepositWalletedDTO dto, string userEmail);
        public Task Withdraw(WithdrawWalletDTO dto, string userEmail);
        public Task DecreaseBalance(WalletDb wallet, double amount);
        public Task IncreaseBalance(WalletDb wallet, double bounty);
    }
}
