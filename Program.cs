using SadRogue.Primitives;
using Console = SadConsole.Console;
using ResidentSurvivor;

namespace Setup{
    class Program{
        static void Main(string[] args){
            System.Console.WriteLine("Hello, World!");

            // Setup the engine and create the main window.
            Game.Setup(80,20);

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }
    }  
}
