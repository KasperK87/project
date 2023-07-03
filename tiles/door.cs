namespace ResidentSurvivor
{
    public class Door : GameObject
    {
        Boolean Walkable { get; set; }
        Char Symbol { get; set; }

        bool IsOpen { get; set; }

        public Door(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Door";
                this.Walkable = false;
                this.Symbol = '+';
                this.IsOpen = false;
        }

        public override void Interact(){
            this.IsOpen = true;
        }

        public override void Update(TimeSpan delta){
            if (IsOpen){
                this.Appearance.Glyph = (int) TileType.DoorOpen;
                this.Walkable = true;
            } else {
                this.Appearance.Glyph = (int) TileType.Door;
                this.Walkable = false;
            }
            base.Update(delta);
        }
    }
}
