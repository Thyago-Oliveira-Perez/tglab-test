using TgLab.Domain.DTOs.User;
using TgLab.Application.User.Services;
using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Application.Auth.Services;
using TgLab.Tests.Bet.Services.Mock;
using TgLab.Domain.Exceptions.User;
using TgLab.Domain.Interfaces.Auth;
using TgLab.Domain.Interfaces.Wallet;
using TgLab.Domain.Interfaces.User;
using TgLab.Application.Transaction.Services;
using TgLab.Domain.Interfaces.Transaction;

namespace TgLab.Tests.User.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private TgLabContext _context;
        private IUserService _userService;
        private IWalletService _walletService;
        private ITransactionService _transactionService;
        private ICryptService _cryptService;
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
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Given_WhenUserIsUnder18_Should_Return_Exception()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                BirthDate = DateTime.Now.AddYears(-17)
            };

            // Act & Assert
            Assert.ThrowsAsync<Under18Exception>(async () => await _userService.Create(createUserDto));
        }

        [Test]
        public async Task Given_WhenUserIsOver18_Should_Create_A_User()
        {
            // Arrange
            var dto = new CreateUserDTO
            {
                Name = "Adult User",
                Email = "adult@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            // Act
            await _userService.Create(dto);

            // Assert
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            Assert.That(user, Is.Not.Null);
        }
    }
}
