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
        private ProcessState previousState; 
        public Dungeon dungeon;
        public Floor currentFloor;
        public Console? menu;
        public Console statusScreen;
        public Console massageScreen;
        public Pause pauseScreen; 
        public Inventory inventoryScreen;

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
            
            currentFloor = dungeon.getCurrentFloor();
           
            //this is the main game screen
            this.Children.Add(currentFloor);

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

            //Creating Pause Screen
            pauseScreen = new Pause(60,20, 0);
            pauseScreen.Position = new Point(60-30, 20-10);
            pauseScreen.Hide();
            this.Children.Add(pauseScreen);

            //Creating Inventory Screen
            inventoryScreen = new Inventory(currentFloor.getPlayer());

            //Creating splashscreen Screen
            Console splashScreen = new SplashScreen(120,40); 

            this.Children.Add(splashScreen);

            // The UIManager becomes the only
            // screen that SadConsole processes
            //Parent = SadConsole.Game.Instance.Screen;
        }
        public override void Update(TimeSpan timeElapsed)
        {
            if (currentState == ProcessState.Active){
                
                statusScreen.Clear();
                
                statusScreen.Print(1,1, "Current turn: " + currentFloor.turn.ToString());

                //Added clear space to remove numbers when the string gets shorter
                statusScreen.Print(1,3, "HP: " + currentFloor.getPlayer().GetSadComponent<IComponent_Entity>().currHP + "/" + 
                    currentFloor.getPlayer().GetSadComponent<IComponent_Entity>().maxHP  + "    ");

                statusScreen.Print(1,4, "Gold: " + currentFloor.getPlayer().getGold());

                //basic test of components, this allows you to store attributes and
                //use them at will
                /*
                IComponent_updater com = currentFloor.getPlayer().GetSadComponent<IComponent_updater>();
                statusScreen.Print(1,3, "HP: " + com.HP + "/" + currentFloor.getPlayer().maxHP + "    ");
                */

                currentFloor.IsFocused = true;

                //fixes the UI opening the pause screen
                //on the same keypress as the pause 
                //screen closes on
                if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.Escape) &&
                    previousState == currentState){
                    currentState = ProcessState.Paused;
                } else if (SadConsole.Game.Instance.Keyboard.IsKeyReleased(SadConsole.Input.Keys.I) &&
                    previousState == currentState){
                    currentState = ProcessState.Inventory;
                }   

            } else if (currentState == ProcessState.Paused){
                if (!pauseScreen.IsVisible)
                    pauseScreen.Show();
                pauseScreen.IsFocused = true;
                currentFloor.IsFocused = false;

            } else if (currentState == ProcessState.Terminated){
                //kill all screens, and show gameover screen
                //this.Children.Add(new Menu(120,40));
                this.Children.Add(new Gameover(120,40,currentFloor.getPlayer().getGold()));

                this.Children.Remove(currentFloor);
                this.Children.Remove(statusScreen);
                this.Children.Remove(massageScreen);

                currentState = ProcessState.Paused;
            } else if (currentState == ProcessState.Inventory){
                if (!inventoryScreen.IsVisible)
                    inventoryScreen.Show();
                inventoryScreen.IsFocused = true;
                currentFloor.IsFocused = false;
            }

            previousState = currentState;
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
                World currentFloor = new World(80,29);
                currentFloor.Position = new Point(1,1);
                currentFloor.DefaultBackground = Color.Black;
                Game.Instance.Screen.Children.Add(currentFloor);  
                */

                //hides the menu
                this.Hide();
            };

            //adding a quit button
            SadConsole.UI.Controls.Button quitButton = new SadConsole.UI.Controls.Button(13,3);
            quitButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            quitButton.Position = new Point(w/2-6,h/2+6);
            quitButton.Text = "QUIT ";
            
            quitButton.Click += (x, y) => {
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            
            this.Controls.Add(startButton);
            this.Controls.Add(quitButton);

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