using Microsoft.EntityFrameworkCore;
using TgLab.Application.Auth.Interfaces;
using TgLab.Application.Auth.Services;
using TgLab.Application.Bet.DTOs;
using TgLab.Application.Bet.Interfaces;
using TgLab.Application.Bet.Services;
using TgLab.Application.User.DTOs;
using TgLab.Application.User.Interfaces;
using TgLab.Application.User.Services;
using TgLab.Application.Wallet.Interfaces;
using TgLab.Application.Wallet.Services;
using TgLab.Domain.Enums;
using TgLab.Infrastructure.Context;
using WalletDb = TgLab.Domain.Models.Wallet;

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
            _betService = new BetService(_context, _userService);
        }

        [Test]
        public void Given_Default_Should_Decrease_User_Balance()
        {
            // Arrange
            var expected = 90;
            var betAmount = 10;
            var userDto = new CreateUserDTO
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = "test*login*password",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            _userService.Create(userDto);
            var user = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);

            var createGambleDTO = new CreateGambleDTO()
            {
                WalletId = user.Wallets.First().Id,
                Amount = betAmount
            };

            // Act
            _betService.Create(createGambleDTO, user.Email);

            user = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);

            // Assert
            Assert.That(user.Wallets.First().Balance == expected);
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
                Currency = Currency.BRL,
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
                Currency = Currency.BRL,
                UserId = 1
            };

            // Act
            var actual = _betService.InvalidBet(createGambleDTO, walletDb);

            // Assert
            Assert.That(actual, Is.False);
        }
    }
}
