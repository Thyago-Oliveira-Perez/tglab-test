using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Domain.DTOs.Wallet;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Application.Transaction.Services;
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.Enums;

namespace TgLab.Tests.User.Services
{
    [TestFixture]
    public class WalletServiceTests
    {
        private TgLabContext _context;
        private IWalletService _walletService;
        private ITransactionService _transactionService;
        private InMemoryNotificationService _notificationService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
            .Options;

            _context = new TgLabContext(options);
            _notificationService = new InMemoryNotificationService();
            _transactionService = new TransactionService(_context);
            _walletService = new WalletService(_context, _notificationService, _transactionService);
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
            Assert.That(addedWallet, Is.Not.Null);
        }

        [Test]
        public async Task IncreaseBalance_ShouldIncreaseWalletBalanceAndSendNotification()
        {
            // Arrange
            var increaseAmount = 50.0;
            var wallet = new WalletDb() 
            { 
                Balance = 100.0,
                Currency = Currency.BRL.Value
            };
            
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            // Act
            await _walletService.IncreaseBalance(wallet, increaseAmount);

            // Assert
            var updatedWallet = _context.Wallets.Find(wallet.Id);
            Assert.That(updatedWallet.Balance, Is.EqualTo(150.0));
        }

        [Test]
        public async Task DecreaseBalance_ShouldDecreaseWalletBalanceAndSendNotification()
        {
            // Arrange
            var decreaseAmount = 30.0;
            var wallet = new WalletDb()
            { 
                Balance = 100.0,
                Currency = Currency.BRL.Value
            };
            
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            // Act
            await _walletService.DecreaseBalance(wallet, decreaseAmount);

            // Assert
            var updatedWallet = _context.Wallets.Find(wallet.Id);
            Assert.That(updatedWallet.Balance, Is.EqualTo(70.0));
        }
    }
}
