using TgLab.Application.User.DTOs;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Services;
using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TgLab.Application.Wallet.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.User.Interfaces;
using TgLab.Application.Auth.Interfaces;
using TgLab.Application.Auth.Services;

namespace TgLab.Tests.User.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private TgLabContext _context;
        private IUserService _userService;
        private IWalletService _walletService;
        private ICryptService _cryptService;

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
            Assert.IsNotNull(user);
        }
    }
}
