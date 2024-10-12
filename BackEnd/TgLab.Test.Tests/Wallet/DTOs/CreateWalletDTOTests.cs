using TgLab.Domain.DTOs.Wallet;
using TgLab.Domain.Enums;

namespace TgLab.Tests.Wallet.DTOs
{
    [TestFixture]
    public class CreateWalletDTOTests
    {
        [TestFixture]
        public class CreateDefaultWallet
        {
            [Test]
            public void Given_Default_Should_Create_Wallet_Currency_BRL()
            {
                // Arrange
                var userId = 1;
                var expectedCurrency = Currency.BRL;

                var sut = new CreateWalletDTO();

                // Act
                var actual = sut.CreateDefaultWallet();

                // Assert
                Assert.That(expectedCurrency == actual.Currency, "Currency should be set to BRL.");
            }

            [Test]
            public void Given_Default_Should_Create_Wallet_Balance_100()
            {
                // Arrange
                var userId = 1;
                var expectedBalance = 100;

                var sut = new CreateWalletDTO();

                // Act
                var actual = sut.CreateDefaultWallet();

                // Assert
                Assert.That(expectedBalance == actual.Balance, "Balance should be set to 100.");
            }
        }
    }
}
