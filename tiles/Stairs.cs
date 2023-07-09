namespace ResidentSurvivor
{
    public class Stairs : GameObject
    {
        private bool IsUp;

        public Stairs(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex, bool setIsUp):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Stairs";
                this.Walkable = false;

                IsUp = setIsUp;

                this.Appearance.Glyph = IsUp ? (int) TileType.UpStairs : (int) TileType.DownStairs;
        }

        public override void Update(TimeSpan delta){
            base.Update(delta);
        }

        public override void Interact()
        {   
            //Mock up code for stairs
            Game.Instance.Screen.Children.Remove(Game.UIManager.dungeon.getCurrentLevel());

            if(!IsUp){
                Game.UIManager.dungeon.setLevel(Game.UIManager.dungeon.currentLevel+1);    
            } else {
                Game.UIManager.dungeon.setLevel(Game.UIManager.dungeon.currentLevel-1); 
            }

            Game.UIManager.newWorld = Game.UIManager.dungeon.getCurrentLevel();
            Game.UIManager.Children.Add(Game.UIManager.newWorld);
            base.Interact();
        }
    }
}