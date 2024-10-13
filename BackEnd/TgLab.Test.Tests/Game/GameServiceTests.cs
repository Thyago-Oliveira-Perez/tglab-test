using TgLab.Application.Game;

namespace TgLab.Tests.Game
{
    [TestFixture]
    public class GameServiceTests
    {
        [Test]
        public void CheckBetResult_Should_Return_True_Or_False()
        {
            // Act
            var result = GameService.CheckBetResult();

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
        }

        [Test]
        public void CalcBounty_Should_Return_Correct_Bounty_Amount()
        {
            // Arrange
            decimal betAmount = 100m;

            // Act
            var bounty = GameService.CalcBounty(betAmount);

            // Assert
            Assert.That(bounty, Is.EqualTo(200m));
        }
    }
}
