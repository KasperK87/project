namespace ResidentSurvivor{
    class Rat : GameObject {

        public Rat(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 1;
              this.currHP = 1;
              this.speed = 1;
        }
    }
}

