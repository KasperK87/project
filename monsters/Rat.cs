namespace ResidentSurvivor{
    class Rat : GameObject {
        private RogueSharp.Path? _cells;
        private bool followingPath;

        private UInt64 turn;



        public Rat(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 1;
              this.currHP = 1;
              this.speed = 1;
              this.damage = 1;

              followingPath = false;
        }

        public override void Update(TimeSpan delta){

            if (this.turn < Game.UIManager.newWorld.turn){
                followPath();
                this.turn = Game.UIManager.newWorld.turn;
            }

            if (!Game.UIManager.newWorld.DungeonMap.IsInFov(this.Position.X, this.Position.Y)){
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                this.Appearance.Foreground = SadRogue.Primitives.Color.White;
                _cells = Game.UIManager.newWorld.pathToPlayerFrom(this.Position.X, this.Position.Y);
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
                        this.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);     
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
    }
}

