using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TgLab.Application.Auth.Services;
using TgLab.Infrastructure.Context;
using TgLab.Domain.Auth;
using TgLab.Domain.Interfaces.Auth;
using UserDb = TgLab.Domain.Models.User;

namespace TgLab.Tests.Auth.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private TgLabContext _context;
        private IConfiguration _configuration;
        private IAuthService _authService;
        private ICryptService _cryptService;

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
            _cryptService = new CryptService();
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

            var user = new UserDb
            {
                Name = "Test Login User",
                Email = "test@login.com",
                Password = _cryptService.HashPassword("test*login*password"),
                BirthDate = DateTime.Now.AddYears(-20)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var actual = await _authService.Login(loginDto);

            // Assert
            Assert.That(actual.Token, Is.Not.Empty, "User logged");
        }
    }
}
