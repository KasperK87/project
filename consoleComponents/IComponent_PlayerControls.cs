using SadConsole;
using SadRogue.Primitives;

namespace ResidentSurvivor{

    //IComponent_PlayerControls is a component that can be added to any entity
    //it will give the entity player controls
    class IComponent_PlayerControls : SadConsole.Components.InputConsoleComponent{
        public bool followingPath{get; set;}
        
        public Point mouseLoc = new Point(0,0);

        //parent objects used to couple the controls of the playrt
        //with the rest of the game
        public SadConsole.Entities.Entity parent;
        Level console;


        private bool preKeyDown; 

        //used to determine if the player should be running
        private TimeSpan timeStampRun = TimeSpan.Zero;
        
        //also get a reference to the dungeonconsole
        public IComponent_PlayerControls(SadConsole.Entities.Entity setParent, Level setConsole){
            this.parent = setParent;
            this.console = setConsole;
            
            followingPath = false;
        }

        public void ProcessMouse(SadConsole.Input.MouseScreenObjectState info){
            
            //System.Console.WriteLine(info.Mouse.LeftClicked);

            if (info.Mouse.LeftClicked){
                followingPath = !followingPath;
            }

            mouseLoc = info.CellPosition;

            if (followingPath) followPath(console._cells);
        }

        private void followPath(RogueSharp.Path? _cells){
            if ( _cells != null)
            {
                //timeStampRun = TimeSpan.Zero;
                try {
                    //_cells.TryStepForward();
                    _cells.StepForward();
                    System.Console.WriteLine(_cells.CurrentStep.X +"," + _cells.CurrentStep.Y);
                    GameObject obj = (GameObject)console.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (obj != null && obj.Walkable){
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y); 
                    } else if(obj != null){ 
                        obj.Interact();
                        System.Console.WriteLine("Player Attack");
                        if (obj.GetSadComponent<IComponent_Entity>() != null){
                            obj.GetSadComponent<IComponent_Entity>().currHP -= parent.GetSadComponent<IComponent_Entity>().damage;
                        }
                        throw new RogueSharp.NoMoreStepsException();
                    } else {
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);                  
                        console.pathXtoY(_cells.End.X, _cells.End.Y);
                        console.turn++;
                    }
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    followingPath = false;
                }
            }
        }
        
        public override void ProcessKeyboard(SadConsole.IScreenObject obj, 
            SadConsole.Input.Keyboard info, out bool flag){

            // Forward the keyboard data to the entity to handle the movement code.
            // We could detect if the users hit ESC and popup a menu or something.
            // By not setting the entity as the active object, twe let this
            // "game level" (the console we're hosting the entity on) determine if
            // the keyboard data should be sent to the entity.

            // Process logic for moving the entity.
            bool keyHit = false;
            //if the player is holding down a movement key
            //we want to run, by repeating the movement
            bool run = false;

            Point oldPosition = parent.Position;
            Point newPosition = (0, 0);

            // Process UP/DOWN movements
            if (info.IsKeyDown(SadConsole.Input.Keys.Up) || info.IsKeyDown(SadConsole.Input.Keys.NumPad8))
            {
                newPosition = parent.Position + (0, -1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.Down) || info.IsKeyDown(SadConsole.Input.Keys.NumPad2))
            {
                newPosition = parent.Position + (0, 1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.Left) || info.IsKeyDown(SadConsole.Input.Keys.NumPad4))
            {
                newPosition = parent.Position + (-1, 0);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.Right) || info.IsKeyDown(SadConsole.Input.Keys.NumPad6))
            {
                newPosition = parent.Position + (1, 0);
                keyHit = true;
            }

            // Process diagonal movements
            if (info.IsKeyDown(SadConsole.Input.Keys.NumPad7)){
                newPosition = parent.Position + (-1, -1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad9)){
                newPosition = parent.Position + (1, -1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad1)){
                newPosition = parent.Position + (-1, 1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad3)){
                newPosition = parent.Position + (1, 1);
                keyHit = true;
            } else if (info.IsKeyDown(SadConsole.Input.Keys.NumPad5)){
                newPosition = parent.Position + (0, 0);
                keyHit = true;
            }

            //do nothing
            if (info.IsKeyDown(SadConsole.Input.Keys.NumPad5))
            {
                keyHit = true;
            }

            //DEBUG
            /*
            if (info.IsKeyPressed(SadConsole.Input.Keys.Q))
            {
                Game.Instance.Screen.Children.Remove(Game.UIManager.dungeon.getCurrentLevel());

                //ternary operator, you can switch between levels
                Game.UIManager.dungeon.setLevel(Game.UIManager.dungeon.currentLevel == 0 ? 1 : 0);
                Game.UIManager.newWorld = Game.UIManager.dungeon.getCurrentLevel();

                Game.UIManager.Children.Add(Game.UIManager.newWorld);
            }
            */
            
            if(preKeyDown && keyHit && Game.UIManager.newWorld.timer >= TimeSpan.FromMilliseconds(500)+timeStampRun){
                run = true;
            } else if (!preKeyDown && !keyHit && !followingPath){
                timeStampRun = Game.UIManager.newWorld.timer;
                //System.Console.WriteLine("RESET TIMER");
            } 
            
            // If a movement key was pressed
            if ((keyHit && !preKeyDown && !followingPath) || run)
            {
                // Check if the new position is valid
                if (Game.UIManager.newWorld.Surface.Area.Contains(newPosition) && Game.UIManager.newWorld.GetDungeonMap().GetCell(newPosition.X, newPosition.Y).IsWalkable){
                    //check is there is a monster
                    var monster = Game.UIManager.newWorld.GetMonsterAt(newPosition.X, newPosition.Y);
                    if (monster == null || parent.Position == newPosition){
                        parent.Position = newPosition;
                        Game.UIManager.newWorld.pathXtoY(mouseLoc.X, mouseLoc.Y);
                    } else {
                        System.Console.WriteLine("Player Attack");
                        if (monster.GetSadComponent<IComponent_Entity>() != null){
                            monster.GetSadComponent<IComponent_Entity>().currHP -= parent.GetSadComponent<IComponent_Entity>().damage;
                        } else {
                            GameObject gameobj = (GameObject)monster;
                            if (gameobj.Walkable){
                                parent.Position = newPosition;
                            } else {
                                gameobj.Interact();
                            }
                        }
                    }
                    preKeyDown = keyHit;
                    //World instance should control turn progression
                    //turn++;
                    Game.UIManager.newWorld.turn++;
                    flag = true;
                }
            }

            if (keyHit) {
                followingPath = false;
                //_cells should also be part of this instance
                //should it really??? 
                Game.UIManager.newWorld._cells = null;
            }

            // You could have multiple entities in the game for example, and change
            // which entity gets keyboard commands.

            preKeyDown = keyHit;
            flag = false;
        }

    public override void ProcessMouse(SadConsole.IScreenObject obj, 
            SadConsole.Input.MouseScreenObjectState info, out bool flag){

            flag = true;
        }
    
    }
}