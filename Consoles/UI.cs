using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor
{
     public class UIManager : ScreenObject{
     // Creates/Holds/Destroys all consoles used in the game
     // and makes consoles easily addressable from a central place.

        public ProcessState currentState;
        public Dungeon dungeon;
        public Floor newWorld;
        public Console? menu;
        public Console statusScreen;
        public Console massageScreen;

        public UIManager()
        {
             //run tests && should be static
            DungeonTest.test1();
            DungeonTest.test2();
            DungeonTest.test3();

            currentState = ProcessState.Active;

            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            //Creating Dungeon
            dungeon = new Dungeon(16);
            
            newWorld = dungeon.getCurrentFloor();
           
            //this is the main game screen
            this.Children.Add(newWorld);

            statusScreen = new Console(40, 29){
                DefaultBackground = Color.AnsiCyan,
                Position = new Point(1,1),
            };

            this.Children.Add(statusScreen);

            massageScreen = new Console(120, 10){
                DefaultBackground = Color.AnsiRed,
                Position = new Point(1,31),
            };

            this.Children.Add(massageScreen);

            Console splashScreen = new SplashScreen(120,40); 

            this.Children.Add(splashScreen);

            // The UIManager becomes the only
            // screen that SadConsole processes
            //Parent = SadConsole.Game.Instance.Screen;
        }
        public override void Update(TimeSpan timeElapsed)
        {
            if (currentState == ProcessState.Active){
                
                statusScreen.Print(1,1, "Current turn: " + newWorld.turn.ToString());

                //Added clear space to remove numbers when the string gets shorter
                statusScreen.Print(1,3, "HP: " + newWorld.getPlayer().GetSadComponent<IComponent_Entity>().currHP + "/" + 
                    newWorld.getPlayer().GetSadComponent<IComponent_Entity>().maxHP  + "    ");

                //basic test of components, this allows you to store attributes and
                //use them at will
                /*
                IComponent_updater com = newWorld.getPlayer().GetSadComponent<IComponent_updater>();
                statusScreen.Print(1,3, "HP: " + com.HP + "/" + newWorld.getPlayer().maxHP + "    ");
                */

            } else if (currentState == ProcessState.Paused){
               
            } else if (currentState == ProcessState.Terminated){
                //kill all screens, and show gameover screen
                this.Children.Remove(newWorld);
                this.Children.Remove(statusScreen);
                this.Children.Remove(massageScreen);

                this.Children.Add(new Gameover(120,40));
            }

            base.Update(timeElapsed);
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

    public class Menu : SadConsole.UI.Window{
        //private Console menuConsole;
        
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        public Menu(int w, int h) : base(w,h){

            fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
            _selectedFont = fonts[0];

            //this.View = new Rectangle(0, 0, 22, 22);
            //this.CanDrag = true;
            this.Title = "MAIN MENU";

            //adding a start button
            SadConsole.UI.Controls.Button startButton = new SadConsole.UI.Controls.Button(13,3);
            //startButton.Theme = new SadConsole.UI.Themes.ButtonTheme('[',']');
            startButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            //NOT VALID WHY?
            //startButton.SetThemeColors((SadConsole.UI.Colors.ColorNames.Orange);
            
            startButton.Position = new Point(w/2-6,h/2+2);
            startButton.Text = "START";
            startButton.Click += (x, y) => {
                
                // World is created in UIManeger 
                // and will appears when we hide 
                // this menu;

                // This code is needed if we change 
                // how world is created
                /*
                World newWorld = new World(80,29);
                newWorld.Position = new Point(1,1);
                newWorld.DefaultBackground = Color.Black;
                Game.Instance.Screen.Children.Add(newWorld);  
                */

                //hides the menu
                this.Hide();
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
        }
    }
}