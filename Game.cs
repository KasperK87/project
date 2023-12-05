using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
    public enum ProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated,
        Inventory
    }

    class Game : SadConsole.Game{
        // Managers (see UI.cs)
       public static UIManager UIManager = new UIManager();

       private Game(){
            //gameInstance is a singleton use setup(width, height)
            UIManager = new UIManager();
       } 

       //This is the main loop
       public static void Setup(int w, int h){
            Create(w, h);
            
            Instance.OnStart = () => {
                //UIManager = new UIManager();
                Game.Instance.Screen = UIManager;
                Game.Instance.DestroyDefaultStartingConsole();
            };

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.Instance.FrameUpdate += Update;
       }

       // allowing some game logic during frame updates
       // quit game with escape key, and f5 to toggle fullscreen
        private static void Update(object? sender, GameHost? host)
        {
            // Called each logic update
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.F5)){
                SadConsole.Game.Instance.ToggleFullScreen();
            }

            if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.Escape)){
                //SadConsole.Game.Instance.Dispose();
                //SadConsole.Game.Instance.MonoGameInstance.Exit();
            }      
        }
    }
}