using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
    class DungeonTest
    {
        public void test1()
        {
            Dungeon dungeon = new Dungeon(16);

            // arrange
            dungeon.setLevel(15);
            int expected = 15;

            // act
            int actual = dungeon.getCurrentLevel();

            // assert
            System.Console.WriteLine("Test 1: " + (expected == actual));
            
        }
    }
}