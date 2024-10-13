using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TgLab.Application.Auth.Services;
using TgLab.Application.Bet.Services;
using TgLab.Application.Game;
using TgLab.Application.User.Services;
using TgLab.Application.Wallet.Services;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using TgLab.Domain.DTOs.Bet;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Interfaces.Auth;
using TgLab.Domain.Interfaces.Bet;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.User;
using WalletDb = TgLab.Domain.Models.Wallet;
using BetDb = TgLab.Domain.Models.Bet;
using UserDb = TgLab.Domain.Models.User;
using TgLab.Domain.Interfaces.Transaction;
using TgLab.Application.Transaction.Services;
using TgLab.Domain.Exceptions.Bet;

namespace TgLab.Tests.Bet.Services
{
    [TestFixture]
    public class BetServiceTests
    {
        private TgLabContext _context;
        private IUserService _userService;
        private IWalletService _walletService;
        private ICryptService _cryptService;
        private IBetService _betService;
        private ITransactionService _transactionService;
        private ILogger<BetService> _logger;
        private IServiceScopeFactory _scopeFactory;
        private IServiceProvider _serviceProvider;
        private GameService _gameService;
        private CancellationTokenSource _cancellationTokenSource;
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
            _cryptService = new CryptService();
            _userService = new UserService(_context, _walletService, _cryptService);

            // Configurando o logger
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<BetService>();

            // Configuração do IServiceProvider e IServiceScopeFactory
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(_context);
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IWalletService, WalletService>();
            serviceCollection.AddTransient<ICryptService, CryptService>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            // Inicializando o GameService e o BetService
            _gameService = new GameService(_logger, _scopeFactory);
            _betService = new BetService(_context, _userService, _walletService, _transactionService, _gameService);

            // CancellationToken para parar o serviço de background após cada teste
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _cancellationTokenSource.Cancel();
        }

        [Test]
        public void Given_InvalidBalance_Should_Return_False()
        {
            // Arrange
            var betAmount = 10;

            var createGambleDTO = new CreateGambleDTO()
            {
                WalletId = 1,
                Amount = betAmount
            };

            var walletDb = new WalletDb()
            {
                Id = 1,
                Balance = 0,
                Currency = Currency.BRL.Value,
                UserId = 1
            };

            // Act
            var actual = _betService.InvalidBet(createGambleDTO, walletDb);

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Given_ValidBalance_Should_Return_False()
        {
            // Arrange
            var betAmount = 10;

            var createGambleDTO = new CreateGambleDTO()
            {
                WalletId = 1,
                Amount = betAmount
            };

            var walletDb = new WalletDb()
            {
                Id = 1,
                Balance = 100,
                Currency = Currency.BRL.Value,
                UserId = 1
            };

            // Act
            var actual = _betService.InvalidBet(createGambleDTO, walletDb);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public async Task Given_Default_Should_Return_List_Of_Bets_Based_On_WalletId()
        {
            // Arrange
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

            var bets = new List<BetDb>()
            { 
                new() 
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 2,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                }
            };

            _context.Users.Add(user);
            _context.Wallets.AddRange(wallets);
            _context.Bets.AddRange(bets);

            _context.SaveChanges();

            // Act
            var actual = await _betService.ListBetsByWalletId(1, user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items, Has.Count.EqualTo(3), "There is bets in the database");
        }

        [Test]
        public async Task Given_Default_Should_Return_All_Bets()
        {
            // Arrange
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

            var bets = new List<BetDb>()
            { 
                new() 
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 1,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                },
                new()
                {
                   WalletId = 2,
                   Amount =  10,
                   Bounty = 20,
                   Time = DateTime.Now,
                   Stage = BetStage.EXECUTED.Value
                }
            };

            _context.Users.Add(user);
            _context.Wallets.AddRange(wallets);
            _context.Bets.AddRange(bets);

            _context.SaveChanges();

            // Act
            var actual = await _betService.ListAll(user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items, Has.Count.EqualTo(4), "There are bets in the database");
        }

        [Test]
        public async Task Given_Default_Should_Cancel_A_Bet_Based_On_Id()
        {
            // Arrange
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

            var bet = new BetDb()
            {
                WalletId = 1,
                Amount =  10,
                Bounty = 20,
                Time = DateTime.Now,
                Stage = BetStage.SENT.Value
            };

            _context.Users.Add(user);
            _context.Wallets.AddRange(wallets);
            var result = _context.Bets.Add(bet);

            _context.SaveChanges();

            // Act
            await _betService.Cancel(result.Entity);

            var actual = _context.Bets.FirstOrDefault(b => b.Id == 1);

            // Assert
            Assert.That(actual.Stage, Is.EqualTo(BetStage.CANCELLED.Value), "There are bets in the database");
        }

        [Test]
        public async Task Given_Already_Cancelled_Bet_Should_Throw_Exception()
        {
            // Arrange
            var user = new UserDb
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            var wallet = new WalletDb()
            {
                UserId = 1,
                Balance = 1000,
                Currency = Currency.BRL.Value,
            };

            var bet = new BetDb()
            {
                WalletId = 1,
                Amount = 10,
                Bounty = 20,
                Time = DateTime.Now,
                Stage = BetStage.CANCELLED.Value
            };

            _context.Users.Add(user);
            _context.Wallets.Add(wallet);
            var result = _context.Bets.Add(bet);

            _context.SaveChanges();

            // Act && Assert
            Assert.ThrowsAsync<AllReadyCancelled>(async () => await _betService.Cancel(result.Entity));
        }
    }
}