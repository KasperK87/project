using SadConsole;
using SadRogue.Primitives;

namespace ResidentSurvivor{
    class IComponent_PlayerControls : SadConsole.Components.InputConsoleComponent{
        public bool followingPath{get; set;}
        
        private Point mouseLoc = new Point(0,0);
        private SadConsole.Entities.Entity parent;
        private bool preKeyDown; 

        //used to determine if the player should be running
        private TimeSpan timeStampRun = TimeSpan.Zero;
        

        public IComponent_PlayerControls(SadConsole.Entities.Entity setParent){
            this.parent = setParent;
            
            followingPath = false;
        }

        public override void ProcessMouse(SadConsole.IScreenObject obj, 
            SadConsole.Input.MouseScreenObjectState info, out bool flag){

            flag = true;
        }

        public void ProcessMouse(SadConsole.Input.MouseScreenObjectState info){
            
            //System.Console.WriteLine(info.Mouse.LeftClicked);

            if (info.Mouse.LeftClicked){
                followingPath = !followingPath;
            }
        }

        //will be implemented when refactered
        /*
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
        */

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
                System.Console.WriteLine("UP");
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

            //System.Console.WriteLine("KeyHit: " + keyHit);
            //System.Console.WriteLine("preKeydown: " + preKeyDown);
            //System.Console.WriteLine("timer " + Game.UIManager.newWorld.timer );
            
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
                if (Game.UIManager.newWorld.Surface.Area.Contains(newPosition) && Game.UIManager.newWorld.DungeonMap.GetCell(newPosition.X, newPosition.Y).IsWalkable){
                    //check is there is a monster
                    var monster = Game.UIManager.newWorld.GetMonsterAt(newPosition.X, newPosition.Y);
                    if (monster == null || parent.Position == newPosition){
                        parent.Position = newPosition;
                        Game.UIManager.newWorld.pathXtoY(mouseLoc.X, mouseLoc.Y);
                    } else {
                        System.Console.WriteLine("Player Attack");
                        
                        monster.GetSadComponent<IComponent_Entity>().currHP -= parent.GetSadComponent<IComponent_Entity>().damage;
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
                Game.UIManager.newWorld._cells = null;
            }

            // You could have multiple entities in the game for example, and change
            // which entity gets keyboard commands.

            preKeyDown = keyHit;
            flag = false;
        }

    }
}