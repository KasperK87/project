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
       }

       private static void Init(){
            ScreenObject container = new ScreenObject();
            Game.Instance.Screen = container;
            Game.Instance.DestroyDefaultStartingConsole();

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
       }
    }

    class Entity{

    }   
}