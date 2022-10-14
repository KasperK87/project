using SadConsole;
using SadRogue.Primitives;

namespace Setup{
    class Program{
        static void Main(string[] args){
            System.Console.WriteLine("Hello, World!");

            // Setup the engine and create the main window.
            Game.Create(60,20);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            Game.Instance.StartingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.StartingConsole.Font);
            Game.Instance.StartingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, Mirror.None);
            Game.Instance.StartingConsole.Print(4, 4, "Hello from SadConsole");
        }
    }
    // See https://aka.ms/new-console-template for more information   


}
