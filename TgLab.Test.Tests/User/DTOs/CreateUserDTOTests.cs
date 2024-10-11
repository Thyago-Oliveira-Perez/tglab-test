using TgLab.Application.User.DTOs;

namespace TgLab.Tests.User.DTOs
{
    [TestFixture]
    public class CreateUserDTOTests
    {

        [TestFixture]
        public class IsAdult
        {
            [Test]
            public void Given_UserOver18_Should_Return_True()
            {
                // Arrange
                var expected = true;
                var sut = new CreateUserDTO()
                {
                    Name = "User Test",
                    Email = "test@email.com",
                    Password = "password",
                    BirthDate = new DateTime(2000, 01, 01),
                };

                // Act
                var actual = sut.IsAdult();

                // Assert
                Assert.That(expected == actual,"User is Adult");
            }

            [Test]
            public void Given_UserUnder18_Should_Return_False()
            {
                // Arrange
                var expected = false;
                var sut = new CreateUserDTO()
                {
                    Name = "User Test",
                    Email = "test@email.com",
                    Password = "password",
                    BirthDate = new DateTime(2020, 01, 01),
                };

                // Act
                var actual = sut.IsAdult();

                // Assert
                Assert.That(expected == actual, "User is Adult");
            }
        }
    }
}