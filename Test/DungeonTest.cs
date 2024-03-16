using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using Xunit;

namespace ResidentSurvivor
{
    public class DungeonTest
    {
        [Fact]
        public void DungeonInit()
        {
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
          
            Dungeon dungeon = new Dungeon(16);
            Assert.True(dungeon != null);  
        }

        [Fact]
        public void DungeonSetLevel()
        {
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            
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
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            
           
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
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(200);
            int expected = 0;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            Assert.True(expected == actual);
        }
    }
}