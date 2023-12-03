namespace ResidentSurvivor {
    public class Player : GameObject {
        //Used only for highscore
        private uint _amountOfGold = 0;

        private GameObject _equippedWeaponBackend = null;
        private GameObject _equippedWeapon {get => _equippedWeaponBackend; set {
            _equippedWeaponBackend = value;
            this.GetSadComponents<IComponent_Entity>().First().damage = value.damage;
        }}

        private GameObject _equippedArmor = null;

        private GameObject[] _inventory = new GameObject[4];

        public Player(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 10;
              this.currHP = 10;
              this.speed = 1;
              this.damage = 3;
              this.Walkable = true;
        }

        public bool AddItemToInventory(GameObject item){
            for (int i = 0; i < _inventory.Length; i++){
                if (_inventory[i] == null){
                    _inventory[i] = item;
                    System.Console.WriteLine("You picked up a " + item.Name);
                    if (item.type == "Weapon"){
                        if (_equippedWeapon == null){
                            _equippedWeapon = item;
                            System.Console.WriteLine("You equipped a " + item.Name);
                        }
                    }
                    return true;
                }
            }
            System.Console.WriteLine("Your inventory is full!");
            return false;
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