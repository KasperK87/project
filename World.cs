using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    public class World : Console {
        public UInt64 turn;
        private static GameObject player;
        private bool followingPath;
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
            //sets current turn:
            turn = 0;

            entityManager = new SadConsole.Entities.Renderer();

            mouseLoc = new Point(0,0);

            timer = TimeSpan.Zero;
            preKeyDown = false;

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

            //create player & populate dungeon
            SadComponents.Add(entityManager);
            foreach ( RogueSharp.Cell cell in DungeonMap.GetAllCells() )
                if (cell.IsWalkable){
                    CreatePlayer(cell.X, cell.Y);
                    //break;

                    if (Random.Next(100) == 0){
                        Rat rat = new Rat(
                            Color.White, Color.Transparent, 100, 99);

                        rat.Position = new Point(cell.X,cell.Y);
                        entityManager.Add(rat);
                    }
                }
 
                entityManager.Add(player);

            

            
            followingPath = false;

            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            //var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            //var normalFont = fontMaster

            this.Font = fontMaster;

            

            
            //DungeonMap = new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy(120, 40, 20, 30, 15, Random);
        }

        // Create a player using SadConsole's Entity class
        private static void CreatePlayer(int x, int y)
        {
            player = new GameObject(
                Color.White, Color.Transparent, 1, 100);

            player.Position = new Point(x,y);
        }

        public override void Update(TimeSpan delta){
            this.IsFocused = true;
            timer += delta;
              
            DungeonMap.UpdatePlayerFieldOfView(player);

            if (followingPath) followPath();

            //updates all entities (GameObject player)
            entityManager.Update(this, delta);

            

            //View.WithCenter(player.Position);
            
            this.View = new Rectangle(player.Position.X-20, player.Position.Y-10, 40, 20);
            pathXtoY(mouseLoc.X, mouseLoc.Y);
        }

        public override void Render(TimeSpan delta){
            base.Render(delta);

            this.Clear();

            DungeonMap.Draw(this);

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
            } else if (!preKeyDown && !keyHit && !followingPath) {
                timer = TimeSpan.Zero;
            }

            // If a movement key was pressed
            if ((keyHit && !preKeyDown && !followingPath) || run)
            {
                // Check if the new position is valid
                // DIRTY CHECK ON DRAW SURFACE FOR COLLICTION CHECK, NEED 
                // REFACTORING
                if (Surface.Area.Contains(newPosition) && DungeonMap.GetCell(newPosition.X, newPosition.Y).IsWalkable)
                        //Surface.GetBackground(newPosition.X, newPosition.Y) == Color.Blue)
                {
                    // Entity moved. Let's draw a trail of where they moved from.
                    //Surface.SetGlyph(player.Position.X, player.Position.Y, 250);
                    player.Position = newPosition;
                    pathXtoY(mouseLoc.X, mouseLoc.Y);
                    preKeyDown = keyHit;
                    turn++;
                    return true;
                }
            }

            if (keyHit) {
                followingPath = false;
                _cells = null;
            }


            // You could have multiple entities in the game for example, and change
            // which entity gets keyboard commands.

            preKeyDown = keyHit;
            return false;
        }
    
        public override bool ProcessMouse(SadConsole.Input.MouseScreenObjectState info){

            //if(mouseLoc != info.CellPosition) pathXtoY();
            
            if (!followingPath){
                mouseLoc = info.CellPosition;
            }

            if (info.Mouse.LeftClicked){
                followingPath = !followingPath;
            }
            

            return false;
        }

        //pathfinder method
        //PROOF OF CONCEPTs
        //private IEnumerable<RogueSharp.Cell> _cells;
        private RogueSharp.Path _cells;
        //should be renamed to better reflex use:
        //player move to point
        public void pathXtoY(int destX, int destY){
            if (player != null){
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap );

            try {
            _cells = _pathFinder.ShortestPath( DungeonMap.GetCell
                (player.Position.X, player.Position.Y),
                DungeonMap.GetCell( destX, destY ) );
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
    
        private void followPath(){
            if ( _cells != null && timer >= TimeSpan.FromMilliseconds(100))
            {
                timer = TimeSpan.Zero;
                try {
                    _cells.StepForward();
                    player.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);                  
                    pathXtoY(_cells.End.X, _cells.End.Y);
                    turn++;
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    followingPath = false;
                }
            }
        }
    }
}