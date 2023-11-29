namespace ResidentSurvivor{

    //IComponent_Entity is a component that can be added to any entity
    //it will make the entity hostile to the player
    //moving towards the player and attacking when in range
    class IComponent_Hostile : SadConsole.Components.UpdateComponent {
        SadConsole.Entities.Entity parent;
        private RogueSharp.Path? _cells;
        //private bool followingPath;
        private UInt64 turn;
        private IComponent_Entity entity;



        public IComponent_Hostile(SadConsole.Entities.Entity setParent, IComponent_Entity setEntity){
            //followingPath = false;

            this.parent = setParent;
            this.entity = setEntity;
        }

        
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            if (this.turn < Game.UIManager.currentFloor.turn && entity.currHP > 0){
                followPath();
                this.turn = Game.UIManager.currentFloor.turn;
            }
            
            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
                _cells = Game.UIManager.currentFloor.pathToPlayerFrom(parent.Position.X, parent.Position.Y);
            }
        }

        private void followPath(){
            if ( _cells != null)
            { 
                try {
                    _cells.StepForward();
                    //check is there is a monster
                    var monster = Game.UIManager.currentFloor.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (monster == null){
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);     
                    } else {
                        GameObject gameobj = (GameObject)monster;
                        if (gameobj.Walkable){
                            parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);;
                        } else if (gameobj.isPlayer){
                            System.Console.WriteLine("Monster Attack!!!");
                            Attack(monster);
                        } else {
                            gameobj.Interact();
                        }
                        
                    }             
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    //followingPath = false;
                }
            }
        }

        public void Attack(SadConsole.Entities.Entity target){
            if (target.GetSadComponent<IComponent_Entity>() != null){
                target.GetSadComponent<IComponent_Entity>().currHP -= parent.GetSadComponent<IComponent_Entity>().damage; ;
            } else {
                GameObject obj = (GameObject)target;
                obj.Interact();
            }
            System.Console.WriteLine("Monster Attack!!!");
        }
    }
}