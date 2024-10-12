using TgLab.Application.Wallet.DTOs;

namespace TgLab.Application.Wallet.Interfaces
{
    public interface IWalletService
    {
        public Task Create(CreateWalletDTO dto);
    }
}
