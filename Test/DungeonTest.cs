using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using Xunit;

namespace ResidentSurvivor
{
    public class DungeonTest
    {
        Game game;
        public DungeonTest()
        {
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
        }

        [Fact]
        public void DungeonInit()
        {
            Dungeon dungeon = new Dungeon(16);
            Assert.True(dungeon != null);  
        }

        [Fact]
        public void DungeonSetLevel()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(3);
            int expected = 3;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            Assert.True(expected == actual);
        }
    
        [Fact]
        public void DungeonSetNegativ()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(-1);
            int expected = 0;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            Assert.True(expected == actual);

        }
        
        [Fact]
        public void DungeonSetOutOfBounds()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(200);
            int expected = 0;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            Assert.True(expected == actual);
        }

        public void Dispose()
        {
            ResidentSurvivor.Game.Instance.MonoGameInstance.Exit();
        }
    }
}