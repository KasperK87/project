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
            
            ResidentSurvivor.Game.Instance.OnStart = () => {
                Dungeon dungeon = new Dungeon(16);
                Assert.True(dungeon != null);
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            ResidentSurvivor.Game.Instance.Run();
        }

        [Fact]
        public void DungeonSetLevel()
        {
            ResidentSurvivor.Game.Setup(120, 40);
            
            ResidentSurvivor.Game.Instance.OnStart = () => {
                Dungeon dungeon = new Dungeon(16);

                // arrange
                dungeon.setLevel(3);
                int expected = 3;

                // act
                int actual = dungeon.getCurrentLevel();

                // assert
                Assert.True(expected == actual);
                
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            ResidentSurvivor.Game.Instance.Run();
        }
        public static void test1()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(3);
            int expected = 3;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            System.Console.WriteLine("Test 1: " + (expected == actual));
            
        }

        public static void test2()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(-1);
            int expected = 0;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            System.Console.WriteLine("Test 2: " + (expected == actual));
            
        }

        public static void test3()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(200);
            int expected = 0;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            System.Console.WriteLine("Test 3: " + (expected == actual));
            
        }
    }
}