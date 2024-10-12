using TgLab.Application.Auth.DTOs;
using TgLab.Application.Auth.Interfaces;
using TgLab.Application.Auth.Services;
using TgLab.Application.User.DTOs;

namespace TgLab.Tests.Auth.Services
{
    [TestFixture]
    public class CryptServiceTests
    {
        [Test]
        public async Task Given_Default_Should_Hash_Password()
        {
            // Arrange
            var password = "password";
            var sut = new CryptService();

            // Act
            var actual = sut.HashPassword(password);

            // Assert
            Assert.That(actual.Length > password.Length, "Password hashed");
        }

        [Test]
        public async Task Given_Default_Should_Return_False()
        {
            // Arrange
            var expected = false;
            var sut = new CryptService();
            var password = "password";
            var savedPassword = sut.HashPassword("password");

            // Act
            var actual = sut.InvalidPassword(password, savedPassword);

            // Assert
            Assert.That(actual == expected, "User logged");
        }

        [Test]
        public async Task Given_Different_Password_Should_Return_True()
        {
            // Arrange
            var expected = true;
            var sut = new CryptService();
            var password = "password";
            var savedPassword = sut.HashPassword("password*");

            // Act
            var actual = sut.InvalidPassword(password, savedPassword);

            // Assert
            Assert.That(actual == expected, "User logged");
        }
    }
}
