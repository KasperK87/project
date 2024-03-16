using Microsoft.Xna.Framework;
using Xunit;

namespace ResidentSurvivorTest
{
    [Collection("Sequential")]
    public class ConsoleTest
    {
        [Fact]
        public void ConsoleInit()
        {
            if (ResidentSurvivor.Game.Instance == null){
                ResidentSurvivor.Game.Setup(120, 40);  
                ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            }

            SadConsole.Console console = new SadConsole.Console(120, 40);

            Assert.NotNull(console);
        }
    }
}