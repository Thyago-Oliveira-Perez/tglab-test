using TgLab.Domain.Enums;

namespace TgLab.Domain.DTOs.Wallet
{
    public class CreateWalletDTO
    {
        public int Balance { get; set; }
        public Currency Currency { get; set; }

        public CreateWalletDTO CreateDefaultWallet()
        {
            this.Balance = 100;
            this.Currency = Currency.BRL;

            return this;
        }
    }
}
