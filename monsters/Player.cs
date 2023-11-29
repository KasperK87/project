namespace ResidentSurvivor {
    public class Player : GameObject {
        //Used only for highscore
        private uint _amountOfGold = 0;
        public Player(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 10;
              this.currHP = 10;
              this.speed = 1;
              this.damage = 3;
              this.Walkable = true;
        }

        public void addGold(uint amount){
            if (amount > 0)
                _amountOfGold += amount;
        }

        public uint getGold(){
            return _amountOfGold;
        }
    }
}