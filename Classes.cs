using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
    class Game : SadConsole.Game{
       private Game(){
            //gameInstance is a singleton use setup(width, height)
       } 

       public static void Setup(int w, int h){
            Create(w, h);
            Instance.OnStart = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.Instance.FrameUpdate += Update;
       }

       private static void Init(){
            //ScreenObject container = new ScreenObject();
            Game.Instance.Screen = new SplashScreen();
            Game.Instance.DestroyDefaultStartingConsole();

            /*
            // First console
            Console console1 = new Console(60, 14);
            console1.Position = new Point(3, 2);
            console1.DefaultBackground = Color.AnsiCyan;
            console1.Clear();
            console1.Print(1, 1, "Type on me!");
            console1.Cursor.Position = new Point(1, 2);
            console1.Cursor.IsEnabled = true;
            console1.Cursor.IsVisible = true;
            console1.Cursor.MouseClickReposition = true;
            console1.IsFocused = true;

            container.Children.Add(console1);
            */
       }

       // allowing some game logic during frame updates
        private static void Update(object sender, GameHost host)
        {
            // Called each logic update
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.F5)){
                SadConsole.Game.Instance.ToggleFullScreen();
            }

            if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.Escape)){
                //SadConsole.Game.Instance.Dispose();
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            }
            
        }
    }

    
     public class UIManager : ScreenObject{
     // Creates/Holds/Destroys all consoles used in the game
     // and makes consoles easily addressable from a central place.
        public Console mapConsole;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Game.Instance.Screen;
        }
    }

    class SplashScreen : ScreenObject{
        private TimeSpan timer;
        private Console splash;
        public SplashScreen(){
            timer = TimeSpan.Zero;

            splash = new Console(20,20);
            splash.Position = new Point(0,0);
            splash.DefaultBackground = Color.AnsiCyan;

            this.Children.Add(splash);
        }
        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            timer += delta;

            if(timer >= TimeSpan.FromSeconds(3)){
                Game.Instance.Screen = new Menu();
            }
        }

        public override void Render(TimeSpan delta)
        {
            base.Render(delta);

            splash.Print(1, 1, "Resident Survivor");
        }
    }

    class Menu : ScreenObject{
        private Console menuConsole;
        private SadConsole.UI.Window menuWindow;
        public Menu(){

            menuConsole = new Console(20,20);

            menuWindow = new SadConsole.UI.Window(22,22);
            menuWindow.View = new Rectangle(0, 0, 22, 22);
            menuWindow.CanDrag = true;
            menuWindow.Title = "MENU";

            SadConsole.UI.Controls.Button closeButton = new SadConsole.UI.Controls.Button(3, 1);

            //Add the close button to the Window's list of UI elements
            menuWindow.Controls.Add(closeButton);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "[X]";



            menuConsole.Position = new Point(1,1);
            menuConsole.DefaultBackground = Color.Red;

            menuConsole.View = new Rectangle(0, 0, 20, 20);

            menuWindow.Children.Add(menuConsole);            

            this.Children.Add(menuWindow);

            menuWindow.Show();
        }

        public override void Render(TimeSpan delta)
        {
            base.Render(delta);

            menuConsole.Print(1, 1, "MENU");
        }
    }

    class Entity{

    }   
}