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
using WalletDb = TgLab.Domain.Models.Wallet;
using TgLab.Domain.DTOs.User;
using TgLab.Domain.DTOs.Bet;
using TgLab.Domain.Interfaces.Notification;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Interfaces.Auth;
using TgLab.Domain.Interfaces.Bet;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.User;

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
            _walletService = new WalletService(_context, _notificationService);
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
            _betService = new BetService(_context, _userService, _gameService);

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

            var gamble1 = new CreateGambleDTO()
            {
                WalletId = walletId,
                Amount = 10
            };

            var gamble2 = new CreateGambleDTO()
            {
                WalletId = walletId,
                Amount = 20
            };

            // Act
            await _betService.Create(gamble1, user.Email);
            await _betService.Create(gamble2, user.Email);

            var actual = await _betService.ListBetsByWalletId(walletId, user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items.Count(), Is.GreaterThan(0), "There is bets in the database");
        }

        [Test]
        public async Task Given_Default_Should_Return_All_Bets()
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

            var gamble1 = new CreateGambleDTO()
            {
                WalletId = walletId,
                Amount = 10
            };

            var gamble2 = new CreateGambleDTO()
            {
                WalletId = walletId,
                Amount = 20
            };

            // Act
            await _betService.Create(gamble1, user.Email);
            await _betService.Create(gamble2, user.Email);

            var actual = await _betService.ListAll(user.Email, 1, 10);

            // Assert
            Assert.That(actual.Items.Count(), Is.GreaterThan(0), "There are bets in the database");
        }

        [Test]
        public async Task Given_Default_Should_Cancel_A_Bet_Based_On_Id()
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

            var gamble1 = new CreateGambleDTO()
            {
                WalletId = walletId,
                Amount = 10
            };

            // Act
            await _betService.Create(gamble1, user.Email);

            await _betService.Cancel(1, user.Email);

            var actual = _context.Bets.FirstOrDefault(b => b.Id == 1);

            // Assert
            Assert.That(actual.Stage, Is.EqualTo(BetStage.CANCELLED.Value), "There are bets in the database");
        }
    }
}