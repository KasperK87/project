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
        public GameObject parent;
        Floor console;


        private bool preKeyDown; 
        private bool performAction = false;

        //used to determine if the player should be running
        private TimeSpan timeStampRun = TimeSpan.Zero;
        
        public IComponent_PlayerControls(SadConsole.Entities.Entity setParent, Floor setConsole){
            this.parent = (GameObject)setParent;
            this.parent.Name = "Player";
            this.console = setConsole;
            
            followingPath = false;
        }

        public void ProcessMouse(SadConsole.Input.MouseScreenObjectState info){
            
            //sort the gameobjects in the status screen
            //so that the index of the gameobject is the same
            //as the index of the entity in the game
            Game.UIManager.statusScreen.sortGameObjects(); 
            
            //System.Console.WriteLine(info.Mouse.LeftClicked);
            //if mouse is hovering over an entity from the status screen
            //set that entity as the target
            if (Game.UIManager.statusScreen._selectedEntity >= 0){
                    mouseLoc = Game.UIManager.statusScreen.gameObjects[Game.UIManager.statusScreen._selectedEntity-6].Position;     
            } else {
                mouseLoc = info.CellPosition;
            }

            if (info.Mouse.LeftClicked){
                followingPath = !followingPath;
                
                //this is a hack to allow the player to wait when clicked on
                if (info.CellPosition == parent.Position){
                    Game.UIManager.currentFloor.turn++;
                }

                //if the player clicks on a tile next to them
                //and there is a gameobject there interact with it
                if ((Math.Abs(mouseLoc.X-parent.Position.X) <= 1 && 
                    Math.Abs(mouseLoc.Y-parent.Position.Y) <= 1) &&
                    Game.UIManager.currentFloor.GetMonsterAt(mouseLoc.X, mouseLoc.Y) != null) {
                        if (Math.Abs(mouseLoc.X-parent.Position.X) == 0 &&
                            Math.Abs(mouseLoc.Y-parent.Position.Y) == 0) return; 
                                GameObject obj = (GameObject)Game.UIManager.currentFloor.GetMonsterAt(mouseLoc.X, mouseLoc.Y);
                                if (obj != null){
                            
                                //This is done to allow items which should be picked up
                                //to not be picked up when the player clicks on them
                                //but to be picked up when the player walks over them
                                if (obj.GetSadComponent<IComponent_Entity>() != null){
                                    parent.Attack(obj);
                                    followingPath = false;
                                    Game.UIManager.currentFloor.turn++;
                                } else if(obj.Interact()){
                                    followingPath = false;
                                    Game.UIManager.currentFloor.turn++;
                                }
                                //when the gameobject is not interactable
                                //and not attackable, it is a item and
                                //should be moved over and picked up
                            
                        }
                }
            }

            if (followingPath) followPath(console._cells);

            //DEBUG
            if (Game.UIManager.currentFloor != null)
            {
                if (Game.UIManager.currentFloor.GetEntitiesAt(info.CellPosition.X, info.CellPosition.Y) != null)
                {
                    var entity = Game.UIManager.currentFloor.GetEntitiesAt(info.CellPosition.X, info.CellPosition.Y)[0];
                    if (entity.HasSadComponent<IComponent_Entity>(out var component))
                    {
                        System.Console.WriteLine(entity.Name + ": " + component.state);
                    }
                }
            }
        }

        private void followPath(RogueSharp.Path? _cells){
            if ( _cells != null)
            {
                try {
                    _cells.StepForward();
                    GameObject obj = (GameObject)console.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (obj != null && obj.Walkable){
                        obj.Pickup((Player)parent);
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y); 
                    } else if(obj != null){ 
                        obj.Interact();
                        if (obj.GetSadComponent<IComponent_Entity>() != null){
                            obj.GetSadComponent<IComponent_Entity>().currHP -= parent.GetSadComponent<IComponent_Entity>().damage;
                            Game.UIManager.currentFloor.addBlood(obj.Position.X, obj.Position.Y,1);
                        }
                        console.turn++;
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

            // 'A' key pressed to perform action
            if (info.IsKeyPressed(SadConsole.Input.Keys.A)){
                performAction = true;
                Game.UIManager.massageScreen.Add("Perform Action Where?");
                flag = true;
                return;
            }
            if (performAction){
                //perform action
                Point newPositionAction = parent.Position;
                if (info.KeysPressed.Count > 0)
                    switch (info.KeysPressed[0].Key){
                        case SadConsole.Input.Keys.Up:
                        case SadConsole.Input.Keys.NumPad8:
                            newPositionAction = parent.Position + (0, -1);
                            break;
                        case SadConsole.Input.Keys.Down:
                        case SadConsole.Input.Keys.NumPad2:
                            newPositionAction = parent.Position + (0, 1);
                            break;
                        case SadConsole.Input.Keys.Left:
                        case SadConsole.Input.Keys.NumPad4:
                            newPositionAction = parent.Position + (-1, 0);
                            break;
                        case SadConsole.Input.Keys.Right:
                        case SadConsole.Input.Keys.NumPad6:
                            newPositionAction = parent.Position + (1, 0);
                            break;
                        case SadConsole.Input.Keys.NumPad7:
                        case SadConsole.Input.Keys.Home:
                            newPositionAction = parent.Position + (-1, -1);
                            break;
                        case SadConsole.Input.Keys.NumPad9:
                        case SadConsole.Input.Keys.PageUp:
                            newPositionAction = parent.Position + (1, -1);
                            break;
                        case SadConsole.Input.Keys.NumPad1:
                        case SadConsole.Input.Keys.End:
                            newPositionAction = parent.Position + (-1, 1);
                            break;
                        case SadConsole.Input.Keys.NumPad3:
                        case SadConsole.Input.Keys.PageDown:
                            newPositionAction = parent.Position + (1, 1);
                            break;
                        case SadConsole.Input.Keys.NumPad5:
                            newPositionAction = parent.Position + (0, 0);
                            break;
                }
                
                GameObject entity = (GameObject)Game.UIManager.currentFloor.GetMonsterAt(newPositionAction.X, newPositionAction.Y);

                if (entity != null){
                    if(entity.Interact()){
                        performAction = false;
                        Game.UIManager.currentFloor.turn++;
                        keyHit = true;
                        preKeyDown = keyHit;
                        flag = true;
                        return;
                    }
                }
                
                if (info.HasKeysDown && !info.IsKeyDown(SadConsole.Input.Keys.A)){
                    performAction = false;
                }
                //needed to stop player from running, when performing action
                timeStampRun = Game.UIManager.currentFloor.timer;

                flag = true;
                return;
            }

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

            //player will follow the path when we click enter
            if (info.IsKeyDown(SadConsole.Input.Keys.Enter))
            {
                followingPath = true;
            }

            if(preKeyDown && keyHit && Game.UIManager.currentFloor.timer >= TimeSpan.FromMilliseconds(500)+timeStampRun){
                run = true;
            } else if (!preKeyDown && !keyHit && !followingPath){
                timeStampRun = Game.UIManager.currentFloor.timer;
            } 
            
            // If a movement key was pressed
            if ((keyHit && !preKeyDown && !followingPath) || run)
            {
                // Check if the new position is valid
                if (Game.UIManager.currentFloor.Surface.Area.Contains(newPosition) && Game.UIManager.currentFloor.GetDungeonMap().GetCell(newPosition.X, newPosition.Y).IsWalkable){
                    //check is there is a monster
                    var monster = (GameObject)Game.UIManager.currentFloor.GetMonsterAt(newPosition.X, newPosition.Y);
                    if (monster == null || parent.Position == newPosition){
                        parent.Position = newPosition;
                        Game.UIManager.currentFloor.pathXtoY(mouseLoc.X, mouseLoc.Y);
                    } else {
                        if (monster.GetSadComponent<IComponent_Entity>() != null){                
                            parent.Attack(monster);    
                        } else {
                            GameObject gameobj = (GameObject)monster;
                            if (gameobj.Walkable){
                                gameobj.Pickup((Player)parent);
                                parent.Position = newPosition;
                            } else {
                                gameobj.Interact();
                            }
                        }
                    }
                    preKeyDown = keyHit;
                    //World instance should control turn progression
                    Game.UIManager.currentFloor.turn++;
                    flag = true;
                }
            }

            if (keyHit) {
                followingPath = false;
                Game.UIManager.currentFloor._cells = null;
            }

            preKeyDown = keyHit;
            flag = false;
        }

        public override void ProcessMouse(SadConsole.IScreenObject obj, 
            SadConsole.Input.MouseScreenObjectState info, out bool flag){

            flag = true;
        }
    
    }
}