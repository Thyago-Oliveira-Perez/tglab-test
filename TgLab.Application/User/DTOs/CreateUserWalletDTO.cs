using TgLab.Domain.Enums;

namespace TgLab.Application.User.DTOs
{
    public class CreateUserWalletDTO
    {
        public CreateUserWalletDTO()
        {
            Currency = Currency.BRL;
            Balance = 100;
        }

        public Currency Currency { get; set; }
        public int Balance { get; set; }
    }
}
