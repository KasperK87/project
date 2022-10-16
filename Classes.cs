using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
    class Game : SadConsole.Game{
        // Managers
       public static UIManager? UIManager;
       private Game(){
            //gameInstance is a singleton use setup(width, height)
       } 

       public static void Setup(int w, int h){
            Create(w, h);
            Instance.OnStart = () => {
                UIManager = new UIManager();

                Game.Instance.Screen = UIManager;
                Game.Instance.DestroyDefaultStartingConsole();
            };

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.Instance.FrameUpdate += Update;
       }

       // allowing some game logic during frame updates
        private static void Update(object? sender, GameHost? host)
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
        public Console? mapConsole;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            Console splashScreen = new SplashScreen(120,40); 

            this.Children.Add(splashScreen);

            // The UIManager becomes the only
            // screen that SadConsole processes
            //Parent = SadConsole.Game.Instance.Screen;
        } // Creates all child consoles to be managed
        public void CreateConsoles()
        {
            //mapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), GameLoop.World.CurrentMap.Tiles);
        }
    }

    class SplashScreen : Console{
        private TimeSpan timer;
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        public SplashScreen(int w, int h) : base( w, h){
            timer = TimeSpan.Zero;

            fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
            _selectedFont = fonts[0];

            Position = new Point(0,0);
            DefaultBackground = Color.Black;

        }
        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            timer += delta;

            if(timer >= TimeSpan.FromSeconds(3)){
                Game.Instance.Screen.Children.Add(new Menu(22,22));
                Game.Instance.Screen.Children.Remove(this);
            }
        }

        public override void Render(TimeSpan delta)
        {
            base.Render(delta);
            Surface.PrintTheDraw(0, 4, "Resident", _selectedFont);
            Surface.PrintTheDraw(0, 16, "Survivor", _selectedFont);
            //this.Print(1, 1, "Resident Survivor");
        }
    }

    class Menu : SadConsole.UI.Window{
        private Console menuConsole;

        public Menu(int w, int h) : base(w,h){

            menuConsole = new Console(w-2,h-2);

            this.View = new Rectangle(0, 0, 22, 22);
            this.CanDrag = true;
            this.Title = "MENU";

            SadConsole.UI.Controls.Button closeButton = new SadConsole.UI.Controls.Button(3, 1);

            //Add the close button to the Window's list of UI elements
            this.Controls.Add(closeButton);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "[X]";

            //using a lambda for better readability
            closeButton.Click += (x, y) => {
                this.Parent.Children.Remove(this);
                //this.Hide();
            };

            menuConsole.Position = new Point(1,1);
            menuConsole.DefaultBackground = Color.Red;

            //menuConsole.View = new Rectangle(0, 0, 20, 20);

            this.Children.Add(menuConsole);            

            this.Show();
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