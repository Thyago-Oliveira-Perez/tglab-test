using TgLab.Application.User.DTOs;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Services;
using TgLab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace TgLab.Tests.User.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private TgLabContext _context;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TgLabContext>()
                .UseInMemoryDatabase(databaseName: "TgLab_Test_Database")
                .Options;

            _context = new TgLabContext(options);
            _userService = new UserService(_context);
        }

        [Test]
        public void Given_Default_Should_Create_A_User()
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
        public async Task Create_WhenUserIsAdult_ShouldAddUser()
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
