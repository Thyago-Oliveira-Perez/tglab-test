using TgLab.Domain.Enums;

namespace TgLab.Application.Wallet.DTOs
{
    public class CreateWalletDTO
    {
        public int UserId { get; set; }
        public int Balance { get; set; }
        public Currency Currency { get; set; }

        public CreateWalletDTO CreateDefaultWallet(int userId)
        {
            this.UserId = userId;
            this.Balance = 100;
            this.Currency = Currency.BRL;

            return this;
        }
    }
}
