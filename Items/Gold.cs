//create a gold class that inherits from the gameobject class

namespace ResidentSurvivor
{
    public class Gold : GameObject
    {
        public int value;

        public Gold(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex, int setValue):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Gold";
                this.Walkable = true;
                this.value = setValue;

                this.Appearance.Glyph = (int) TileType.Gold;
        }

        public override void Pickup(Player player){
            this.currHP = 0;
            System.Console.WriteLine("You picked up " + this.value + " gold!");
            player.addGold((uint)this.value);
        }

        public override void Update(TimeSpan delta){
            base.Update(delta);
        }
    }
}