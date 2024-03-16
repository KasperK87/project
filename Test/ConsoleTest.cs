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
            
            ResidentSurvivor.Game.Instance.OnStart = () => {
                SadConsole.Console console = new SadConsole.Console(120, 40);
                Assert.True(console != null);
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };

            ResidentSurvivor.Game.Instance.Run();
        }
    }
}