using Xunit;

namespace ResidentSurvivorTest
{
    public class GameTest
    {
        [Fact]
        public void GameInit()
        {
            ResidentSurvivor.Game.Setup(120, 40);
            Assert.True(ResidentSurvivor.Game.Instance != null);
        }
    }
}