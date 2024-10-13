using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TgLab.Application.Auth.Services;
using TgLab.Domain.DTOs.User;
using TgLab.Application.User.Services;
using TgLab.Application.Wallet.Services;
using TgLab.Infrastructure.Context;
using TgLab.Domain.Auth;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Interfaces.Auth;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.User;

namespace TgLab.Tests.Auth.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private TgLabContext _context;
        private IConfiguration _configuration;
        private IAuthService _authService;
        private IUserService _userService;
        private IWalletService _walletService;
        private ICryptService _cryptService;
        private InMemoryNotificationService _notificationService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
                .Options;

            var configData = new Dictionary<string, string>
            {
                { "Jwt:Key", "D@2$3fF6h8^uGhY!9jKlM12#NqRzT*6Y" },
                { "Jwt:Issuer", "TgLab.Test.Application" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _context = new TgLabContext(options);
            _notificationService = new InMemoryNotificationService();
            _walletService = new WalletService(_context, _notificationService);
            _cryptService = new CryptService();
            _userService = new UserService(_context, _walletService, _cryptService);
            _authService = new AuthService(_context, _configuration, _cryptService);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Given_Default_Should_Login()
        {
            // Arrange
            var loginDto = new LoginDTO()
            {
                Email = "test@login.com",
                Password = "test*login*password"
            };

            var userDto = new CreateUserDTO
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            // Act
            await _userService.Create(userDto);
            var actual = await _authService.Login(loginDto);

            // Assert
            Assert.That(actual.Token.Length > 0, "User logged");
        }
    }
}
