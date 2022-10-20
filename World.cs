using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    class World : Console {
        private static SadConsole.Entities.Entity? player;

        public RogueSharpSadConsoleSamples.Core.DungeonMap DungeonMap;

        private Point mouseLoc;

        private TimeSpan timer;
        private bool preKeyDown; 
        public static RogueSharp.Random.IRandom? Random { get; private set; }


        //int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level
        RogueSharpSadConsoleSamples.Systems.MapGenerator
            mapGenerator = new RogueSharpSadConsoleSamples.Systems.MapGenerator(
                120,40, 10, 10, 5, 1);

        private SadConsole.Entities.Renderer entityManager;
        public World(int w, int h) : base( w, h){
            entityManager = new SadConsole.Entities.Renderer();

            mouseLoc = new Point(0,0);

            timer = TimeSpan.Zero;
            preKeyDown = false;

            SadComponents.Add(entityManager);

            

            // Setup this console to accept keyboard input.
            UseKeyboard = true;
            IsVisible = true;
            UseMouse = true;

            //create dungeon
            //DungeonMap = mapGenerator.CreateMap();
            int seed = (int) DateTime.UtcNow.Ticks;
            Random = new RogueSharp.Random.DotNetRandom( seed );

            RogueSharp.MapCreation.IMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap> mapCreationStrategy =
                new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap>( 80, 29, 100, 7, 3 );
            DungeonMap = mapCreationStrategy.CreateMap();

            foreach ( RogueSharp.Cell cell in DungeonMap.GetAllCells() )
                if (cell.IsWalkable){
                    CreatePlayer(cell.X, cell.Y);
                    break;
                }

            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            //var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            //var normalFont = fontMaster

            this.Font = fontMaster;

            entityManager.Add(player);

            
            //DungeonMap = new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy(120, 40, 20, 30, 15, Random);
        }

        // Create a player using SadConsole's Entity class
        private static void CreatePlayer(int x, int y)
        {
            player = new SadConsole.Entities.Entity(
                Color.White, Color.Transparent, 1, 100);

            player.Position = new Point(x,y);
        }

        public override void Update(TimeSpan delta){
            this.IsFocused = true;
            timer += delta;
              
            DungeonMap.UpdatePlayerFieldOfView(player);

            //View.WithCenter(player.Position);
            
            this.View = new Rectangle(player.Position.X-20, player.Position.Y-10, 40, 20);
        }

        public override void Render(TimeSpan delta){
            base.Render(delta);

            this.Clear();

            DungeonMap.Draw(this);

            pathXtoY();
            drawPath();

            this.SetBackground(player.Position.X, player.Position.Y, Color.Black);
            this.SetBackground(mouseLoc.X, mouseLoc.Y, Color.Yellow);
        }
    
        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            // Forward the keyboard data to the entity to handle the movement code.
            // We could detect if the users hit ESC and popup a menu or something.
            // By not setting the entity as the active object, twe let this
            // "game level" (the console we're hosting the entity on) determine if
            // the keyboard data should be sent to the entity.

            // Process logic for moving the entity.
            bool keyHit = false;
            bool run = false;

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

            if(preKeyDown && keyHit && timer >= TimeSpan.FromMilliseconds(500)){
                run = true;
            } else if (!preKeyDown && !keyHit) {
                timer = TimeSpan.Zero;
            }

            // If a movement key was pressed
            if ((keyHit && !preKeyDown) || run)
            {
                // Check if the new position is valid
                // DIRTY CHECK ON DRAW SURFACE FOR COLLICTION CHECK, NEED 
                // REFACTORING
                if (Surface.Area.Contains(newPosition) &&
                        Surface.GetBackground(newPosition.X, newPosition.Y) == Color.Blue)
                {
                    // Entity moved. Let's draw a trail of where they moved from.
                    //Surface.SetGlyph(player.Position.X, player.Position.Y, 250);
                    player.Position = newPosition;

                    preKeyDown = keyHit;
                    return true;
                }
            }

            // You could have multiple entities in the game for example, and change
            // which entity gets keyboard commands.

        
            preKeyDown = keyHit;
            return false;
        }
    
        public override bool ProcessMouse(SadConsole.Input.MouseScreenObjectState info){

            mouseLoc = info.CellPosition;

            return false;
        }

        //pathfinder method
        //PROOF OF CONCEPTs
        //private IEnumerable<RogueSharp.Cell> _cells;
        private RogueSharp.Path _cells;
        public void pathXtoY(){
            if (player != null){
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap );

            try {
            _cells = _pathFinder.ShortestPath( DungeonMap.GetCell
                (player.Position.X, player.Position.Y),
                DungeonMap.GetCell( mouseLoc.X, mouseLoc.Y ) );
            } catch { 
                
            }
            }

        }

        private void drawPath(){
            if ( _cells != null)
            {
                foreach ( RogueSharp.Cell cell in _cells.Steps)
                {
                    if ( cell != null )
                    {
                        this.SetBackground(cell.X, cell.Y, Color.LightYellow);
                    }
                }
            }
        }
    }
}