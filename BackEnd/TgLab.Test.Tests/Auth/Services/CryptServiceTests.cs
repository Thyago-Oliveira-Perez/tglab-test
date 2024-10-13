using TgLab.Application.Auth.Services;

namespace TgLab.Tests.Auth.Services
{
    [TestFixture]
    public class CryptServiceTests
    {
        [Test]
        public void Given_Default_Should_Hash_Password()
        {
            // Arrange
            var password = "password";
            var sut = new CryptService();

            // Act
            var actual = sut.HashPassword(password);

            // Assert
            Assert.That(actual, Has.Length.GreaterThan(password.Length), "Password hashed");
        }

        [Test]
        public void Given_Default_Should_Return_False()
        {
            // Arrange
            var expected = false;
            var sut = new CryptService();
            var password = "password";
            var savedPassword = sut.HashPassword("password");

            // Act
            var actual = sut.InvalidPassword(password, savedPassword);

            // Assert
            Assert.That(actual, Is.EqualTo(expected), "User logged");
        }

        [Test]
        public void Given_Different_Password_Should_Return_True()
        {
            // Arrange
            var expected = true;
            var sut = new CryptService();
            var password = "password";
            var savedPassword = sut.HashPassword("password*");

            // Act
            var actual = sut.InvalidPassword(password, savedPassword);

            // Assert
            Assert.That(actual, Is.EqualTo(expected), "User logged");
        }
    }
}
