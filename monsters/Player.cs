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

            //adds animation to player
            var anim = new SadConsole.Entities.AnimatedAppearanceComponent();
            
            var frame1 = new SadConsole.ColoredString.ColoredGlyphEffect();
            frame1.Glyph = (int)TileType.Player;
            frame1.Foreground = SadRogue.Primitives.Color.White;
            frame1.Background = SadRogue.Primitives.Color.Transparent;

            var frame2 = new SadConsole.ColoredString.ColoredGlyphEffect();
            frame2.Glyph = (int)75;
            frame2.Foreground = SadRogue.Primitives.Color.White;
            frame2.Background = SadRogue.Primitives.Color.Transparent;

            anim.Frames = new SadConsole.ColoredString.ColoredGlyphEffect[2];

            anim.Frames[0] = frame1;
            anim.Frames[1] = frame2;

            anim.AnimationTime = TimeSpan.FromSeconds(1);
            anim.IsRepeatable = true;

            this.SadComponents.Add(anim);

            if (this.GetSadComponent<SadConsole.Entities.AnimatedAppearanceComponent>() != null){  
                this.GetSadComponent<SadConsole.Entities.AnimatedAppearanceComponent>().Start();
            }
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

        public string[] getInventory(){
            string[] inventory = new string[_inventory.Length];
            for (int i = 0; i < _inventory.Length; i++){
                if (_inventory[i] != null){
                    inventory[i] = _inventory[i].Name;
                }
            }
            return inventory;
        }
        public bool equipItem(int index){
            if (_inventory[index] != null){
                if (_inventory[index].type == "Weapon"){
                    _equippedWeapon = _inventory[index];
                    System.Console.WriteLine("You equipped a " + _inventory[index].Name);
                    return true;
                }
            }
            return false;
        }
        public bool dropItem(int index){
            if (_inventory[index] != null){
                if (_inventory[index] == _equippedWeapon){
                    System.Console.WriteLine("You can't drop your equipped weapon!"); 
                    return false;
                }
                _inventory[index].Position = this.Position;
                Game.UIManager.currentFloor.entityManager.Add(_inventory[index]);                
                _inventory[index] = null;
                return true;
            }
            return false;
        }
    }
}