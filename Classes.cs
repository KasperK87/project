using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
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
                Menu menu = new Menu(120,40);
                
                Game.Instance.Screen.Children.Add(menu);

                Game.Instance.Screen.Children.Remove(this);
            }
        }

        public override void Render(TimeSpan delta)
        {
            this.Clear();

            int offcenteringX = 15;
            int offcenteringY = (int)(timer.TotalMilliseconds/300);
            Surface.PrintTheDraw(2+offcenteringX, this.Height/2-10-offcenteringY, "Resident", _selectedFont);
            Surface.PrintTheDraw(this.Width-80-offcenteringX, this.Height/2-offcenteringY, "Survivor", _selectedFont);

            base.Render(delta);
            //this.Print(1, 1, "Resident Survivor");
        }
    }

    class Menu : SadConsole.UI.Window{
        //private Console menuConsole;
        
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        public Menu(int w, int h) : base(w,h){

            fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
            _selectedFont = fonts[0];

            //this.View = new Rectangle(0, 0, 22, 22);
           //this.CanDrag = true;
            this.Title = "MAIN MENU";
            
            //this.Children.Add(menuConsole);    
            /*
            //Add close button to the Window's list of UI elements
            SadConsole.UI.Controls.Button closeButton = new SadConsole.UI.Controls.Button(3, 1);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "[X]";
            this.Controls.Add(closeButton);

            //using a lambda for better readability
            closeButton.Click += (x, y) => {
                this.Parent.Children.Remove(this);
                //this.Hide();
            };
            */

            //adding a start button
            SadConsole.UI.Controls.Button startButton = new SadConsole.UI.Controls.Button(7,1);
            startButton.Theme = new SadConsole.UI.Themes.ButtonTheme();
            //startButton.SetThemeColors();
            startButton.Position = new Point(7,10);
            startButton.Text = "START";
            startButton.Click += (x, y) => {
                World newWorld = new World(118,38);
                newWorld.Position = new Point(1,1);
                newWorld.DefaultBackground = Color.Black;
                Game.Instance.Screen.Children.Add(newWorld);  
            };
            
            this.Controls.Add(startButton);

            //WHY NOT WORKING?
            this.DefaultBackground = Color.Blue;
            this.DefaultForeground = Color.White;

            //WORK AROUND
            for (int i = 0; i < this.Width; i++)
                for (int j = 0; j < this.Height; j++){
                    this.SetBackground(i,j, Color.Black);
                    this.SetForeground(i,j, Color.Orange);
                }

            this.Show();

            //AbsolutePosition = new Point(
            //    500, 100);
        }

        public override void Render(TimeSpan delta)
        {            
            base.Render(delta);
            int offcentering = 15;
            Surface.PrintTheDraw(2+offcentering, this.Height/2-19, "Resident", _selectedFont);
            Surface.PrintTheDraw(this.Width-80-offcentering, this.Height/2-9, "Survivor", _selectedFont);


            //menuConsole.Print(1, 1, "MENU");
        }
    }
}