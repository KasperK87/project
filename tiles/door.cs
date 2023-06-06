namespace ResidentSurvivor
{
    public class Door : SadConsole.Entities.Entity
    {
        Boolean Walkable { get; set; }
        Char Symbol { get; set; }

        bool IsOpen { get; set; }

        public Door(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Door";
                this.Walkable = false;
                this.Symbol = '+';
        }

        public override void Update(TimeSpan delta){
            if (IsOpen){
                this.Appearance.Glyph = '-';
                this.Walkable = true;
            } else {
                this.Appearance.Glyph = '+';
                this.Walkable = false;
            }
            base.Update(delta);
        }
    }
}
