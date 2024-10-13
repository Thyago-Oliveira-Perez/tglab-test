using WalletCurrency = TgLab.Domain.Enums.Currency;

namespace TgLab.Domain.DTOs.Wallet
{
    public class CreateWalletDTO
    {
        public int Balance { get; set; }
        public string Currency { get; set; }

        public CreateWalletDTO CreateDefaultWallet()
        {
            this.Balance = 100;
            this.Currency = WalletCurrency.BRL.Value;

            return this;
        }
    }
}
