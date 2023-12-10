namespace ResidentSurvivor{

    //IComponent_Entity is a component that can be added to any entity
    //it will make the entity hostile to the player
    //moving towards the player and attacking when in range
    class IComponent_Hostile : SadConsole.Components.UpdateComponent {
        GameObject parent;
        private RogueSharp.Path? _cells;
        //private bool followingPath;
        private UInt64 turn;
        private IComponent_Entity entity;



        public IComponent_Hostile(SadConsole.Entities.Entity setParent, IComponent_Entity setEntity){
            //followingPath = false;

            this.parent = (GameObject)setParent;
            this.entity = setEntity;
        }

        
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            //every turn we do the following once
            if (this.turn < Game.UIManager.currentFloor.turn && entity.currHP > 0){
                if (parent.GetSadComponent<IComponent_Entity>() != null){
                    if (parent.GetSadComponent<IComponent_Entity>().state == entityState.hostile){
                        followPath();                  
                    } else if (Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                        if (_cells != null)
                            if (parent.rand.Next(1,20) > 15 && _cells.Length <= 7)
                                parent.GetSadComponent<IComponent_Entity>().state = entityState.hostile;
                    }
                }
                
                this.turn = Game.UIManager.currentFloor.turn;
            }

            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                //parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
                _cells = Game.UIManager.currentFloor.pathToPlayerFrom(parent.Position.X, parent.Position.Y);
            }
        }

        private void followPath(){
            if ( _cells != null)
            { 
                try {
                    _cells.StepForward();
                    //check is there is a monster
                    var monster = (GameObject)Game.UIManager.currentFloor.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (monster == null){
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);     
                    } else {
                        if (monster.Walkable){
                            parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);;
                        } else if (monster.isPlayer){
                            //System.Console.WriteLine("Monster Attack!!!");
                            Attack(monster);
                        } else {
                            monster.Interact();
                        }
                        
                    }             
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    //followingPath = false;
                }
            }
        }

        public void Attack(GameObject target){
            if (target.GetSadComponent<IComponent_Entity>() != null){
                parent.Attack(target);
            } else {
                target.Interact();
            }
            System.Console.WriteLine("Monster Attack!!!");
        }
    }
}