namespace ResidentSurvivor
{
    public class Stairs : GameObject
    {
        public Stairs(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Stairs";
                this.Walkable = true;
        }
    }
}