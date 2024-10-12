using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Domain.DTOs.Wallet;

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

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Given_Default_Should_Create_Wallet()
        {
            // Arrange
            var userEmail = "test@email.com";
            var wallet = new CreateWalletDTO().CreateDefaultWallet();

            // Act
            var addedWallet = _walletService.Create(wallet, userEmail);

            // Assert
            Assert.IsNotNull(addedWallet);
        }
    }
}
