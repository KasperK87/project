using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    enum TileType : int{
        Empty = 38,
        Player = 1,
        Rat = 376,
        Floor = 27,
        Wall = 284,
        Solid = 76,
        Door = 96,
        DoorOpen = 97,
        UpStairs = 169,
        DownStairs = 170,
        Gold = 282
    }
    public class Floor : Console {
        public UInt64 turn;
        private static Player player = new Player(
                Color.White, Color.Blue, (int) TileType.Player, 100);

        private RogueSharpSadConsoleSamples.Core.DungeonMap DungeonMap;

        //will be refactored away
        public TimeSpan timer;

        public static RogueSharp.Random.IRandom? Random { get; private set; }

        //MapGenerator not used, but could be used to generate maps in the future
        //int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level
        RogueSharpSadConsoleSamples.Systems.MapGenerator
            mapGenerator = new RogueSharpSadConsoleSamples.Systems.MapGenerator(
                120,40, 10, 10, 5, 1);
        
        RogueSharp.Point downStairsLocation;
        RogueSharp.Point upStairsLocation;


        public SadConsole.Entities.Manager entityManager;

        public Floor(int w, int h) : base( w, h){
            //sets current turn:
            turn = 0;

            entityManager = new SadConsole.Entities.Manager();

            //referenced in playerController
            timer = TimeSpan.Zero;

            // Setup this console to accept keyboard input.
            UseKeyboard = true;
            IsVisible = true;
            UseMouse = true;

            //Create dungeon
            //DungeonMap = mapGenerator.CreateMap();
            int seed = (int) DateTime.UtcNow.Ticks;
            Random = new RogueSharp.Random.DotNetRandom( seed );

            //Temporarily using RogueSharp's RandomRoomsMapCreationStrategy
            RogueSharp.MapCreation.IMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap> mapCreationStrategy =
                new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap>( 80, 29, 100, 7, 3 );
            DungeonMap = mapCreationStrategy.CreateMap();

            

            //create player, populate and decorate dungeon
            SadComponents.Add(entityManager);

            //creates player to remove annoying could be null warnings
            player = new Player(
                Color.White, Color.Blue, (int) TileType.Player, 100);

            // List of valid locations where stairs can be placed
            List<RogueSharp.Point> stairsLocations = new List<RogueSharp.Point>();

            foreach ( RogueSharp.Cell cell in DungeonMap.GetAllCells() )
                if (cell.IsWalkable){
                    bool isEmpty = true;
                    //insert door generation here
                    /*DOOR KERNELS IMPLEMENTATION
                    -----------
                    *d*
                    ...
                    -----------
                    .*
                    .d
                    .*
                    -----------
                    */
                    if ((!DungeonMap.GetCell(cell.X-1, cell.Y).IsWalkable && DungeonMap.GetCell(cell.X, cell.Y).IsWalkable &&
                        !DungeonMap.GetCell(cell.X+1, cell.Y).IsWalkable && DungeonMap.GetCell(cell.X-1, cell.Y+1).IsWalkable &&
                        DungeonMap.GetCell(cell.X, cell.Y+1).IsWalkable && DungeonMap.GetCell(cell.X+1, cell.Y+1).IsWalkable) ||
                        (DungeonMap.GetCell(cell.X-1, cell.Y-1).IsWalkable && !DungeonMap.GetCell(cell.X, cell.Y-1).IsWalkable &&
                        DungeonMap.GetCell(cell.X-1, cell.Y).IsWalkable && DungeonMap.GetCell(cell.X, cell.Y).IsWalkable &&
                        DungeonMap.GetCell(cell.X-1, cell.Y+1).IsWalkable && !DungeonMap.GetCell(cell.X, cell.Y+1).IsWalkable))               
                    {
                        if (Random.Next(100) < 33 && isEmpty){
                        DungeonMap.SetCellProperties(cell.X, cell.Y, false, true, false);

                        //changed too stairs for testing
                        Door door = new Door(
                            Color.White, Color.Transparent, (int) TileType.Door, 98, DungeonMap);

                        door.Position = new Point(cell.X,cell.Y);

                        entityManager.Add(door); 
                        isEmpty = false;
                          
                        }
                    }

                    if (Random.Next(50) == 0 && isEmpty){
                        GameObject rat = new GameObject(
                            Color.White, Color.Transparent, (int) TileType.Rat, 99);

                        rat.Name = "Rat";

                        rat.Position = new Point(cell.X,cell.Y);
                        rat.Walkable = false;

                        var entity = new IComponent_Entity(rat, 1, 1, 1, 1);

                        rat.SadComponents.Add(entity);
                        rat.SadComponents.Add(new IComponent_Hostile(rat, entity));
                        entityManager.Add(rat);
                        isEmpty = false;
                        
                    } else if (Random.Next(50) == 0 && isEmpty){
                        Gold gold = new Gold(
                            Color.Yellow, Color.Transparent, (int) TileType.Gold, 98, 
                            Random.Next(10, 100));

                        gold.Position = new Point(cell.X,cell.Y);
                        gold.Walkable = true;

                        //var entity = new IComponent_Entity(gold, 1, 1, 0, 0);
                    
                        //gold.SadComponents.Add(entity);

                        entityManager.Add(gold);
                        isEmpty = false;
                    }
                    CreatePlayer(cell.X, cell.Y);
                } // check to generate stairs
                else if (Random.Next(100) < 101 && cell.X >= 5 && cell.X <= 75 && cell.Y >= 5 && cell.Y <= 25 &&
                    
                        (DungeonMap.GetCell(cell.X-1, cell.Y-1).IsWalkable && !DungeonMap.GetCell(cell.X, cell.Y-1).IsWalkable &&
                        DungeonMap.GetCell(cell.X-1, cell.Y).IsWalkable && !DungeonMap.GetCell(cell.X, cell.Y).IsWalkable &&
                        DungeonMap.GetCell(cell.X-1, cell.Y+1).IsWalkable && !DungeonMap.GetCell(cell.X, cell.Y+1).IsWalkable)){
                    
                    stairsLocations.Add( new RogueSharp.Point( cell.X, cell.Y ) );
                }
            
            // Choose Up Stairs location
            int stairsIndex = Random.Next(stairsLocations.Count-1);
            upStairsLocation = stairsLocations[stairsIndex];

            DungeonMap.SetCellProperties(upStairsLocation.X, upStairsLocation.Y, true, true, false);
                    Stairs stairs = new Stairs(
                        Color.White, Color.Transparent, (int) TileType.UpStairs, 98, 
                        true);
                        //Random.Next(100) < 50 ? true : false);

                    stairs.Position = new Point(upStairsLocation.X,upStairsLocation.Y);

                    entityManager.Add(stairs);
            
            stairsLocations.RemoveAt(stairsIndex);
            
            // Choose Down Stairs location
            downStairsLocation = stairsLocations[Random.Next(stairsLocations.Count-1)];

            DungeonMap.SetCellProperties(downStairsLocation.X, downStairsLocation.Y, true, true, false);
                    Stairs stairs2 = new Stairs(
                        Color.White, Color.Transparent, (int) TileType.DownStairs, 98, 
                        false);
                        //Random.Next(100) < 50 ? true : false);

                    stairs2.Position = new Point(downStairsLocation.X,downStairsLocation.Y);

                    entityManager.Add(stairs2);

            player.SadComponents.Add(new IComponent_Entity(player, 10, 10, 1, 1));
            player.isPlayer = true;
            this.SadComponents.Add(new IComponent_PlayerControls(player, this));

            //System.Console.WriteLine(this.GetSadComponent<IComponent_PlayerControls>().followingPath);
            
            entityManager.Add(player);

            //doesn't work, call from update loop instead 
            //entityManager.DoEntityUpdate = true;

            //followingPath = false;

            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            //var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            //var normalFont = fontMaster

            this.Font = fontMaster;

            //this.FontSize = new SadRogue.Primitives.Point(16,24);;
        }

        // Create a player using SadConsole's Entity class
        private static void CreatePlayer(int x, int y)
        {
            player = new Player(
                Color.White, Color.Transparent, (int) TileType.Player, 100);
            player.Walkable = false;
            player.Position = new Point(x,y);
        }

        public void playerEnterFromUp(GameObject newPlayer, bool up){

            GetSadComponent<IComponent_PlayerControls>().parent = newPlayer;
            entityManager.Remove(getPlayer());
            entityManager.Add(newPlayer);

            if (!up){
                player.Position = new Point(downStairsLocation.X-1, downStairsLocation.Y);
            } else {
                player.Position = new Point(upStairsLocation.X-1, upStairsLocation.Y);
            }
        }

        public Player getPlayer(){
            return (Player)GetSadComponent<IComponent_PlayerControls>().parent;
        }

        public override void Update(TimeSpan delta){
            this.IsFocused = true;
            timer += delta;
              
            DungeonMap.UpdatePlayerFieldOfView(GetSadComponent<IComponent_PlayerControls>().parent);

            //removed as the level shouldn't know if the player is following a path
            //if (GetSadComponent<IComponent_PlayerControls>().followingPath) followPath();

            //updates all entities (GameObject player)
            entityManager.Update(this, delta);

            //View.WithCenter(player.Position);
            
            this.View = new Rectangle(GetSadComponent<IComponent_PlayerControls>().parent.Position.X-20, 
                GetSadComponent<IComponent_PlayerControls>().parent.Position.Y-10, 40, 20);
            if (!GetSadComponent<IComponent_PlayerControls>().followingPath){
                pathXtoY(GetSadComponent<IComponent_PlayerControls>().mouseLoc.X,GetSadComponent<IComponent_PlayerControls>().mouseLoc.Y);
            }

            //If player is dead, game over
            if (player.GetSadComponent<IComponent_Entity>().currHP < 1){
                Game.UIManager.currentState = ProcessState.Terminated;
            }
        }

        public override void Render(TimeSpan delta){
            base.Render(delta);

            this.Clear();

            DungeonMap.Draw(this);

            drawPath();

            this.SetBackground(GetSadComponent<IComponent_PlayerControls>().mouseLoc.X,GetSadComponent<IComponent_PlayerControls>().mouseLoc.Y, Color.Yellow);
        }

        //draws path to mouse
        public override bool ProcessMouse(SadConsole.Input.MouseScreenObjectState info){

            //if(mouseLoc != info.CellPosition) pathXtoY();
            GetSadComponent<IComponent_PlayerControls>().ProcessMouse(info);

            return false;
        }

        //pathfinder method
        //PROOF OF CONCEPTs
        //private IEnumerable<RogueSharp.Cell> _cells;
        public RogueSharp.Path? _cells{get; set;}
        //should be renamed to better reflex use:
        //player move to point
        public void pathXtoY(int destX, int destY){
            if (GetSadComponent<IComponent_PlayerControls>().parent != null && DungeonMap.GetCell(destX, destY).IsWalkable){
              
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap);
            
            try{
            _cells = _pathFinder.ShortestPath( DungeonMap.GetCell
                (GetSadComponent<IComponent_PlayerControls>().parent.Position.X, 
                    GetSadComponent<IComponent_PlayerControls>().parent.Position.Y),
                DungeonMap.GetCell( destX, destY ) );
            } catch (RogueSharp.PathNotFoundException) { 
                _cells = null;
            }
            
            }

        }

        //gives the path to the player from a point
         public RogueSharp.Path? pathToPlayerFrom(int origX, int origY){
            RogueSharp.PathFinder _pathFinder;
            _pathFinder = new RogueSharp.PathFinder( DungeonMap );

            try {
            return _pathFinder.ShortestPath( DungeonMap.GetCell
                (origX, origY),
                DungeonMap.GetCell( GetSadComponent<IComponent_PlayerControls>().parent.Position.X, 
                    GetSadComponent<IComponent_PlayerControls>().parent.Position.Y) );
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
                        this.SetBackground(cell.X, cell.Y, Color.LightYellow.SetAlpha(180));
                    }
                }
                this.SetBackground(_cells.Start.X, _cells.Start.Y, Color.Blue);
            }
        }

        //This is not optimal, might be very buggy
        public SadConsole.Entities.Entity GetMonsterAt( int x, int y ){
            //should crash if entity is not of class GameObject,
            //could use a try catch block to fix.
            //or remove gameobject class and make a 
            //game object component 
            return entityManager.GetEntityAtPosition(new Point(x, y));
        }

        public SadConsole.Entities.Entity[] GetEntitiesAt( int x, int y ){
            //should crash if entity is not of class GameObject,
            //could use a try catch block to fix.
            //or remove gameobject class and make a 
            //game object component 
            return entityManager.GetEntitiesAtPosition(new Point(x, y));
        }

        public RogueSharpSadConsoleSamples.Core.DungeonMap GetDungeonMap(){
            return DungeonMap;
        }
    }

}