using ResidentSurvivor;

namespace Setup{
    class Program{
        static void Main(string[] args){
            System.Console.WriteLine("Hello, World!");

            // Setup the engine and create the main window.
            Game.Setup(80,20);

            // Start the game.
            Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            Game.Instance.Dispose();
        }
    }  
}
