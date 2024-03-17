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
            SadConsole.Console console = new SadConsole.Console(120, 40);

            Assert.NotNull(console);
        }
    }
}