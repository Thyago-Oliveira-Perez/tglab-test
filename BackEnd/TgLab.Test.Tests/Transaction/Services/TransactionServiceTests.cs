using Microsoft.EntityFrameworkCore;
using TgLab.Application.Auth.Interfaces;
using TgLab.Application.Auth.Services;
using TgLab.Application.Bet.DTOs;
using TgLab.Application.Bet.Interfaces;
using TgLab.Application.Bet.Services;
using TgLab.Application.Transactional.Interfaces;
using TgLab.Application.Transactional.Services;
using TgLab.Application.User.DTOs;
using TgLab.Application.User.Interfaces;
using TgLab.Application.User.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Wallet.Services;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using TransactionDb = TgLab.Domain.Models.Transaction;

namespace TgLab.Tests.Transaction.Services
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private TgLabContext _context;
        private IUserService _userService;
        private IWalletService _walletService;
        private ICryptService _cryptService;
        private ITransactionService _transactionalService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
                .Options;

            _context = new TgLabContext(options);
            _walletService = new WalletService(_context);
            _cryptService = new CryptService();
            _userService = new UserService(_context, _walletService, _cryptService);
            _transactionalService = new TransactionService(_context, _userService);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Given_Default_Should_Return_All_Transactions()
        {
            // Arrange
            var userDto = new CreateUserDTO
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            await _userService.Create(userDto);

            var user = _context.Users
                .Include(u => u.Wallets)
                .First(u => u.Email == userDto.Email);

            var walletId = user.Wallets.First().Id;

            var transactions = new List<TransactionDb>()
                {
                    new()
                    {
                        WalletId = walletId,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN
                    },
                    new()
                    {
                        WalletId = walletId,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.LOSS
                    }
                };

            // Act
            _context.Transactions.AddRange(transactions);
            _context.SaveChanges();

            var actual = await _transactionalService.ListAll(user.Email);

            // Assert
            Assert.That(actual.Count(), Is.GreaterThan(0), "There are transactions in the database");
        }

        [Test]
        public async Task Given_Default_Should_Cancel_A_Transaction_Based_On_Id()
        {
            // Arrange
            var userDto0 = new CreateUserDTO
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            var userDto1 = new CreateUserDTO
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            await _userService.Create(userDto0);
            await _userService.Create(userDto1);

            var user0 = _context.Users
                .Include(u => u.Wallets)
                .First(u => u.Email == userDto0.Email);

            var walletId = user0.Wallets.First().Id;

            var transactionsUser0 = new List<TransactionDb>()
            {
                    new()
                    {
                        WalletId = walletId,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN
                    },
                    new()
                    {
                        WalletId = walletId,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.LOSS
                    }
            };

            var transactionsUser1 = new List<TransactionDb>()
            {
                    new()
                    {
                        WalletId = 2,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN
                    },
                    new()
                    {
                        WalletId = 2,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.LOSS
                    }
            };

            // Act
            _context.Transactions.AddRange(transactionsUser0);
            _context.Transactions.AddRange(transactionsUser1);
            _context.SaveChanges();

            var actual = await _transactionalService.ListTransactionsByWalletId(walletId, user0.Email);

            // Assert
            Assert.That(actual.Count(), Is.EqualTo(transactionsUser0.Count), "There are transactionsin the database");
        }
    }
}
