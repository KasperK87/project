namespace ResidentSurvivor
{
    public class Stairs : GameObject
    {
        private bool IsUp;

        //Used to prevent spamming of stairs
        //across levels
        static TimeSpan timer = TimeSpan.Zero;

        public Stairs(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex, bool setIsUp):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Stairs";
                this.Walkable = false;
                this.type = "Stairs";

                IsUp = setIsUp;

                this.Appearance.Glyph = IsUp ? (int) TileType.UpStairs : (int) TileType.DownStairs;
        }

        public override void Update(TimeSpan delta){
            timer += delta;
            base.Update(delta);
        }

        public override bool Interact(){   
            //Mock up code for stairs
            Game.Instance.Screen.Children.Remove(Game.UIManager.dungeon.getCurrentFloor());

            if(!IsUp && TimeSpan.FromMilliseconds(500) < timer){
                Game.UIManager.dungeon.setLevel(Game.UIManager.dungeon.getCurrentLevel()+1);    
            } else if (IsUp && TimeSpan.FromMilliseconds(500) < timer){
                Game.UIManager.dungeon.setLevel(Game.UIManager.dungeon.getCurrentLevel()-1); 
            }

            timer = TimeSpan.Zero;

            Game.UIManager.currentFloor = Game.UIManager.dungeon.getCurrentFloor();
            Game.UIManager.Children.Add(Game.UIManager.currentFloor);
            return true;
        }
    }
}