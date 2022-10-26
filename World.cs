using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    public class World : Console {
        public UInt64 turn;
        private static GameObject player;

        //will be refactored away
        private bool followingPath;
        public RogueSharpSadConsoleSamples.Core.DungeonMap DungeonMap;

        //will be refactored away
        private Point mouseLoc;

        //will be refactored away
        private TimeSpan timer;

        //will be refactored away
        private bool preKeyDown; 
        public static RogueSharp.Random.IRandom? Random { get; private set; }


        //int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level
        RogueSharpSadConsoleSamples.Systems.MapGenerator
            mapGenerator = new RogueSharpSadConsoleSamples.Systems.MapGenerator(
                120,40, 10, 10, 5, 1);

        public SadConsole.Entities.Manager entityManager;
        

        public World(int w, int h) : base( w, h){
            //sets current turn:
            turn = 0;

            entityManager = new SadConsole.Entities.Manager();

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
                        SadConsole.Entities.Entity rat = new SadConsole.Entities.Entity(
                            Color.White, Color.Transparent, 100, 99);

                        rat.Position = new Point(cell.X,cell.Y);

                        var entity = new IComponent_Entity(rat, 1, 1, 1, 1);

                        rat.SadComponents.Add(entity);
                        rat.SadComponents.Add(new IComponent_Hostile(rat, entity));
                        entityManager.Add(rat);
                    }
                }
 
                player.SadComponents.Add(new IComponent_Entity(player, 10, 10, 1, 1));
                entityManager.Add(player);

                //doesn't work, call from update loop instead 
                //entityManager.DoEntityUpdate = true;

            followingPath = false;

            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            //var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            //var normalFont = fontMaster

            this.Font = fontMaster;

            //this.FontSize = new SadRogue.Primitives.Point(16,24);;
        }

        // Create a player using SadConsole's Entity class
        private static void CreatePlayer(int x, int y)
        {
            player = new GameObject(
                Color.White, Color.Blue, 1, 100);

            player.Position = new Point(x,y);
        }

        public GameObject getPlayer(){
            return player;
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
            if (info.IsKeyDown(SadConsole.Input.Keys.Up) || info.IsKeyDown(SadConsole.Input.Keys.NumPad8))
            {
                newPosition = player.Position + (0, -1);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.Down) || info.IsKeyDown(SadConsole.Input.Keys.NumPad2))
            {
                newPosition = player.Position + (0, 1);
                keyHit = true;
            }

            // Process LEFT/RIGHT movements
            if (info.IsKeyDown(SadConsole.Input.Keys.Left) ||info.IsKeyDown(SadConsole.Input.Keys.NumPad4))
            {
                newPosition = player.Position + (-1, 0);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.Right) || info.IsKeyDown(SadConsole.Input.Keys.NumPad6))
            {
                newPosition = player.Position + (1, 0);
                keyHit = true;
            }

            //diagonal movement
            if (info.IsKeyDown(SadConsole.Input.Keys.NumPad7))
            {
                newPosition = player.Position + (-1, -1);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad9))
            {
                newPosition = player.Position + (1, -1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad1))
            {
                newPosition = player.Position + (-1, 1);
                keyHit = true;
            }
            else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad3))
            {
                newPosition = player.Position + (1, 1);
                keyHit = true;
            }

            //Do nothing
            if (info.IsKeyDown(SadConsole.Input.Keys.NumPad5))
            {
                newPosition = player.Position + (0, 0);
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
                if (Surface.Area.Contains(newPosition) && DungeonMap.GetCell(newPosition.X, newPosition.Y).IsWalkable){
                    //check is there is a monster
                    var monster = Game.UIManager.newWorld.GetMonsterAt(newPosition.X, newPosition.Y);
                    if (monster == null || player.Position == newPosition){
                        player.Position = newPosition;
                        pathXtoY(mouseLoc.X, mouseLoc.Y);
                    } else {
                        System.Console.WriteLine("Player Attack");
                        player.Attack(monster);
                    }
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
            if (player != null && DungeonMap.GetCell(destX, destY).IsWalkable){
              
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap );

            _cells = _pathFinder.ShortestPath( DungeonMap.GetCell
                (player.Position.X, player.Position.Y),
                DungeonMap.GetCell( destX, destY ) );
            
            }

        }

        //gives the path to the player from a point
         public RogueSharp.Path pathToPlayerFrom(int origX, int origY){
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap );

            try {
            return _pathFinder.ShortestPath( DungeonMap.GetCell
                (origX, origY),
                DungeonMap.GetCell( player.Position.X, player.Position.Y) );
            } catch (RogueSharp.NoMoreStepsException) { 
                return null;
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
                this.SetBackground(_cells.Start.X, _cells.Start.Y, Color.Blue);
            }
        }
    
        //should be moved into a player component
        private void followPath(){
            if ( _cells != null && timer >= TimeSpan.FromMilliseconds(100))
            {
                timer = TimeSpan.Zero;
                try {
                    //_cells.TryStepForward();
                    _cells.StepForward();
                    System.Console.WriteLine(_cells.CurrentStep.X +"," + _cells.CurrentStep.Y);
                    if (GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y) != null){
                        throw new RogueSharp.NoMoreStepsException();
                    } else {
                        player.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);                  
                        pathXtoY(_cells.End.X, _cells.End.Y);
                        turn++;
                    }
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    followingPath = false;
                }
            }
        }

        //This is not optimal, might be very buggy
        public SadConsole.Entities.Entity GetMonsterAt( int x, int y ){
            //should crash if entity is not of class GameObject,
            //could use a try catch block to fix.
            //or remove gameobject class and make a 
            //game object component 
            return entityManager.GetEntityAtPosition(new Point(x, y));
            /*
            return (GameObject?)entityManager.Entities.FirstOrDefault( m => m.Position.X == x && 
                    m.Position.Y == y );
            */
        }
    }

}