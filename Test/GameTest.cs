using Xunit;

namespace ResidentSurvivorTest
{
    [Collection("Sequential")]
    public class GameTest
    {
        [Fact]
        public void GameInit()
        {
            Assert.True(ResidentSurvivor.Game.Instance != null);
        }
    }
}