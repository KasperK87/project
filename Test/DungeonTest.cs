using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using Xunit;
using ResidentSurvivorTest;

namespace ResidentSurvivor
{
    [Collection("Sequential")]
    public class DungeonTest
    {
        MonoGameFixture fixture;
        public DungeonTest(MonoGameFixture fixture)
        {
            this.fixture = fixture;   
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
    }
}