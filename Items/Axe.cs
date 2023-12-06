namespace ResidentSurvivor {
    public class Axe : GameObject {
        public Axe(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
            this.type = "Weapon";
            this.damage = 10;
            this.Name = "Axe";
            this.Appearance.Glyph = (int) TileType.Axe;
        }
        public override void Pickup(Player player) {
            
            if(player.AddItemToInventory(this)) {
                Game.UIManager.currentFloor.entityManager.Remove(this);
                //this.currHP = 0;
            }
        }

        public override void Update(System.TimeSpan delta){
            base.Update(delta);
        }
    }
}