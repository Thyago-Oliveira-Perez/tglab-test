﻿using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Domain.DTOs.Wallet;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Application.Transaction.Services;

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
            Assert.IsNotNull(addedWallet);
        }
    }
}
