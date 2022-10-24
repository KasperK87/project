namespace ResidentSurvivor{
    class IComponent_Hostile : SadConsole.Components.UpdateComponent {
        GameObject parent;
        private RogueSharp.Path? _cells;
        private bool followingPath;
        private UInt64 turn;



        public IComponent_Hostile(GameObject setParent){
            followingPath = false;

            this.parent = setParent;
        }

        
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            if (this.turn < Game.UIManager.newWorld.turn && parent.currHP > 0){
                followPath();
                this.turn = Game.UIManager.newWorld.turn;
            }
            if (!Game.UIManager.newWorld.DungeonMap.IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
                _cells = Game.UIManager.newWorld.pathToPlayerFrom(parent.Position.X, parent.Position.Y);
            }
        }

        private void followPath(){
            if ( _cells != null)
            { 
                try {
                    _cells.StepForward();
                    //check is there is a monster
                    GameObject? monster = Game.UIManager.newWorld.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (monster == null){
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);     
                    } else {
                        //System.Console.WriteLine("Monster Attack!!!");
                        Attack(monster);
                    }             
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    followingPath = false;
                }
            }
        }

        public void Attack(GameObject target){
            target.currHP -= parent.damage;
        }
    }
}