using ResidentSurvivor;

//entry point to everything
namespace Setup{
    class Program{
        static void Main(string[] args){
            // Setup the engine and create the main window.
            Game.Setup(120,40);

            // Start the game.
            Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            Game.Instance.Dispose();
        }
    }  
}
