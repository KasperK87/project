namespace ResidentSurvivor
{
    public class Door : GameObject
    {
        Char Symbol { get; set; }

        bool IsOpen { get; set; }

        private RogueSharpSadConsoleSamples.Core.DungeonMap hostDungeon;

        public Door(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex, 
            RogueSharpSadConsoleSamples.Core.DungeonMap setDungeon):
            base(c1, c2, SetGlyph, zIndex){
                this.Name = "Door";
                this.Walkable = false;
                this.IsOpen = false;

                hostDungeon = setDungeon;
        }

        public override bool Interact(){
            this.IsOpen = !this.IsOpen;
            if (this.IsOpen)
                hostDungeon.SetCellProperties(this.Position.X, this.Position.Y, true, true);
            else {
                this.Walkable = false;
                hostDungeon.SetCellProperties(this.Position.X, this.Position.Y, false, true);
            }
            return true;
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
