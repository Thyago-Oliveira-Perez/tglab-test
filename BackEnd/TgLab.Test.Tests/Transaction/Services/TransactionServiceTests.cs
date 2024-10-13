using Microsoft.EntityFrameworkCore;
using TgLab.Application.Transaction.Services;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using TgLab.Domain.Interfaces.Transaction;
using TransactionDb = TgLab.Domain.Models.Transaction;
using UserDb = TgLab.Domain.Models.User;
using WalletDb = TgLab.Domain.Models.Wallet;

namespace TgLab.Tests.Transaction.Services
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private TgLabContext _context;
        private ITransactionService _transactionalService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
            .Options;

            _context = new TgLabContext(options);
            _transactionalService = new TransactionService(_context);
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
            var user = new UserDb
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            var wallet = new WalletDb
            {
                UserId = 1,
                Balance = 100,
                Currency = Currency.BRL.Value,
            };

            var transactions = new List<TransactionDb>()
            {
                    new()
                    {
                        WalletId = 1,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN_BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    }
            };

            _context.Users.Add(user);
            _context.Wallets.Add(wallet);
            _context.Transactions.AddRange(transactions);
            _context.SaveChanges();
            
            // Act
            var actual = await _transactionalService.ListAll(user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items, Has.Count.EqualTo(transactions.Count), "There are transactions in the database");
        }

        [Test]
        public async Task Given_Default_Should_Return_All_Transactions_From_Given_Wallet()
        {
            // Arrange
            var user = new UserDb
            {
                Name = "Test Login User",
                Email = "test0@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            var wallets = new List<WalletDb>()
            {
               new()
               {
                    UserId = 1,
                    Balance = 100,
                    Currency = Currency.BRL.Value,
               },
               new()
               {
                    UserId = 1,
                    Balance = 100,
                    Currency = Currency.BRL.Value,
               }
            };

            var transactions = new List<TransactionDb>()
            {
                    new()
                    {
                        WalletId = 1,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN_BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    },
                    new()
                    {
                        WalletId = 2,
                        Amount = 50,
                        Time = DateTime.Now,
                        Type = TransactionType.WIN_BET.Value
                    },
                    new()
                    {
                        WalletId = 2,
                        Amount = 13,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    }
            };


            _context.Users.Add(user);
            _context.Wallets.AddRange(wallets);
            _context.Transactions.AddRange(transactions);
            _context.SaveChanges();

            // Act
            var actual = await _transactionalService.ListTransactionsByWalletId(1, user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items, Has.Count.EqualTo(2), "There are transactionsin the database");
        }

        [Test]
        public void Given_Win_After_Five_Losts_Should_Give_10_Bonus()
        {
            // Arrange
            var expected = 5;
            var user = new UserDb
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            var wallets = new List<WalletDb>()
            {
                new()
                {
                    UserId = 1,
                    Balance = 1000,
                    Currency = Currency.BRL.Value,
                },
                new()
                {
                    UserId = 1,
                    Balance = 10000,
                    Currency = Currency.USD.Value,
                }
            };

            var transactions = new List<TransactionDb>()
            {
                    new()
                    {
                        WalletId = 1,
                        Amount = 10,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 10,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 10,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 10,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    },
                    new()
                    {
                        WalletId = 1,
                        Amount = 10,
                        Time = DateTime.Now,
                        Type = TransactionType.BET.Value
                    }
            };

            _context.Users.Add(user);
            _context.Wallets.AddRange(wallets);
            _context.Transactions.AddRange(transactions);

            _context.SaveChanges();

            // Act
            var actual = _transactionalService.CalcBonus(1, TransactionType.WIN_BET);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
