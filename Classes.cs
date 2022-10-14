using SadConsole;
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
        }
    }

    class SplashScreen : ScreenObject{
        private TimeSpan timer;
        public SplashScreen(){
            timer = TimeSpan.Zero;

            Console splash = new Console(20,20);
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
    }

    class Menu : ScreenObject{
        public Menu(){

            Console splash = new Console(20,20);
            splash.Position = new Point(0,0);
            splash.DefaultBackground = Color.Red;

            this.Children.Add(splash);
        }
    }

    class Entity{

    }   
}