using Xunit;

namespace ResidentSurvivorTest
{
    [Collection("Sequential")]
    public class GameTest
    {
        [Fact]
        public void GameInit()
        {
            if (ResidentSurvivor.Game.Instance == null){
                ResidentSurvivor.Game.Setup(120, 40);  
                ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            }

            Assert.True(ResidentSurvivor.Game.Instance != null);
        }
    }
}