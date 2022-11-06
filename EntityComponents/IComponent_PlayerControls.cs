using SadConsole;
using SadRogue.Primitives;

namespace ResidentSurvivor{
    class IComponent_PlayerControls : SadConsole.Components.InputConsoleComponent{
        public bool followingPath{get; private set;}
        private Point mouseLoc = new Point(0,0);
        private SadConsole.Entities.Entity parent;
        private bool preKeyDown; 
        private TimeSpan timer = TimeSpan.Zero;

        public IComponent_PlayerControls(SadConsole.Entities.Entity setParent){
            this.parent = setParent;
            followingPath = false;
        }

        public override void ProcessMouse(SadConsole.IScreenObject obj, 
            SadConsole.Input.MouseScreenObjectState info, out bool flag){
                
            flag = false;
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
            }

            //do nothing
            if (info.IsKeyDown(SadConsole.Input.Keys.NumPad5))
            {
                keyHit = true;
            }

            System.Console.WriteLine("KeyHit: " + keyHit);
            System.Console.WriteLine("preKeydown: " + preKeyDown);
            System.Console.WriteLine("timer " + this.timer.TotalMilliseconds);
            
            if(preKeyDown && keyHit && Game.UIManager.newWorld.timer >= TimeSpan.FromMilliseconds(500)){
                run = true;
            } else if (!preKeyDown && !keyHit && !followingPath) {
                Game.UIManager.newWorld.timer = TimeSpan.Zero;
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