using Microsoft.Xna.Framework;
using Xunit;

namespace ResidentSurvivorTest
{
    public class ConsoleTest
    {
        [Fact]
        public void ConsoleInit()
        {
            ResidentSurvivor.Game.Setup(120, 40);  
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();

            SadConsole.Console console = new SadConsole.Console(120, 40);

            Assert.NotNull(console);
        }
    }
}