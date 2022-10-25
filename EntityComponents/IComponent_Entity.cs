namespace ResidentSurvivor{
    class IComponent_Entity : SadConsole.Components.UpdateComponent{
        SadConsole.Entities.Entity parent;

        public int maxHP;
        public int currHP{get; set;}
        int speed;
        public int damage{get; set;}

        public IComponent_Entity(SadConsole.Entities.Entity setParent, int setMaxHP, int setCurrHP, int setSpeed, int setDamage){
            this.parent = setParent;

            maxHP = setMaxHP;
            currHP = setCurrHP;
            speed = setSpeed;
            damage = setDamage;
        }
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            if (currHP < 1){
                //this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;  
                Game.UIManager.newWorld.entityManager.Remove(parent);
                System.Console.WriteLine("rat dead");
            }

            if (!Game.UIManager.newWorld.DungeonMap.IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
            }
        }
    }
}