namespace ResidentSurvivor {
    public class Dagger : GameObject {

        public Dagger(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 1;
              this.currHP = 1;
              this.speed = 1;
              this.damage = 2;
              this.Walkable = true;
              this.Name = "Dagger"; 
              this.type = "Weapon";
              this.Appearance.Glyph = (int) TileType.Dagger;
        }

        public override void Pickup(Player player) {
            
            if(player.AddItemToInventory(this)) {
                Game.UIManager.currentFloor.entityManager.Remove(this);
            }
        }

        public override void Update(System.TimeSpan delta){
            base.Update(delta);
        }
    }
}