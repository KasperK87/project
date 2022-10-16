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
            int offcentering = 15;
            Surface.PrintTheDraw(2+offcentering, this.Height/2-10, "Resident", _selectedFont);
            Surface.PrintTheDraw(this.Width-80-offcentering, this.Height/2, "Survivor", _selectedFont);

            //this.Print(1, 1, "Resident Survivor");
        }
    }

    class Menu : SadConsole.UI.Window{
        //private Console menuConsole;
        
        public Menu(int w, int h) : base(w,h){

            //menuConsole = new Console(w-2,h-2);
            //menuConsole.Position = new Point(1,1);
            //menuConsole.DefaultBackground = Color.Red;

            this.View = new Rectangle(0, 0, 22, 22);
            this.CanDrag = true;
            this.Title = "MENU";
            //this.Children.Add(menuConsole);    

            
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

            //adding a start button
            SadConsole.UI.Controls.Button startButton = new SadConsole.UI.Controls.Button(7,1);
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
            this.DefaultBackground = Color.Red;
            this.DefaultForeground = Color.Orange;

            //WORK AROUND
            for (int i = 0; i < this.Width; i++)
                for (int j = 0; j < this.Height; j++)
                    this.SetBackground(i,j, Color.Red);

            this.Show();
        }

        public override void Render(TimeSpan delta)
        {
            base.Render(delta);

            //menuConsole.Print(1, 1, "MENU");
        }
    }

    class World : Console {
        private static SadConsole.Entities.Entity player;

        public RogueSharpSadConsoleSamples.Core.DungeonMap DungeonMap;


        //int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level
        RogueSharpSadConsoleSamples.Systems.MapGenerator
            mapGenerator = new RogueSharpSadConsoleSamples.Systems.MapGenerator(
                120,40, 10, 10, 5, 1);

        private SadConsole.Entities.Renderer entityManager;
        public World(int w, int h) : base( w, h){
            entityManager = new SadConsole.Entities.Renderer();

            SadComponents.Add(entityManager);

            CreatePlayer();

            entityManager.Add(player);

            // Setup this console to accept keyboard input.
            //UseKeyboard = true;
            IsVisible = true;

            //create dungeon
            DungeonMap = mapGenerator.CreateMap();
        }

        // Create a player using SadConsole's Entity class
        private static void CreatePlayer()
        {
            player = new SadConsole.Entities.Entity(
                Color.White, Color.Black, 1, 100);
            player.Position = new Point(5,5);
        }

        public override void Update(TimeSpan delta){
            ProcessKeyboard(SadConsole.Game.Instance.GetKeyboardState());
        }

        public override void Render(TimeSpan delta){
            base.Render(delta);
            
            DungeonMap.Draw(this);

        }
    
        public bool ProcessKeyboard(SadConsole.Input.IKeyboardState info)
        {
            // Forward the keyboard data to the entity to handle the movement code.
            // We could detect if the users hit ESC and popup a menu or something.
            // By not setting the entity as the active object, twe let this
            // "game level" (the console we're hosting the entity on) determine if
            // the keyboard data should be sent to the entity.

            // Process logic for moving the entity.
            bool keyHit = false;
            Point oldPosition = player.Position;
            Point newPosition = (0, 0);

            // Process UP/DOWN movements
            if (info.IsKeyDown(SadConsole.Input.Keys.Up))
            {
                newPosition = player.Position + (0, -1);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.Down))
            {
                newPosition = player.Position + (0, 1);
                keyHit = true;
            }

            // Process LEFT/RIGHT movements
            if (info.IsKeyDown(SadConsole.Input.Keys.Left))
            {
                newPosition = player.Position + (-1, 0);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.Right))
            {
                newPosition = player.Position + (1, 0);
                keyHit = true;
            }

            // If a movement key was pressed
            if (keyHit)
            {
                // Check if the new position is valid
                if (Surface.Area.Contains(newPosition))
                {
                    // Entity moved. Let's draw a trail of where they moved from.
                    Surface.SetGlyph(player.Position.X, player.Position.Y, 250);
                    player.Position = newPosition;

                    return true;
                }
            }

            // You could have multiple entities in the game for example, and change
            // which entity gets keyboard commands.

            return false;
        }
    }

    class Entity{

    }   
}