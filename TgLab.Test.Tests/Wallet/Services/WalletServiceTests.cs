using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Wallet.DTOs;

namespace TgLab.Tests.User.Services
{
    [TestFixture]
    public class WalletServiceTests
    {
        private TgLabContext _context;
        private IWalletService _walletService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
                .Options;

            _context = new TgLabContext(options);
            _walletService = new WalletService(_context);
        }

        [Test]
        public void Given_Default_Should_Create_Wallet()
        {
            // Arrange
            var userId = 1;
            var wallet = new CreateWalletDTO().CreateDefaultWallet(userId);

            // Act
            var addedWallet = _walletService.Create(wallet);

            // Assert
            Assert.IsNotNull(addedWallet);
        }
    }
}
