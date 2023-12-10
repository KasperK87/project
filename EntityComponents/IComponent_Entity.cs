namespace ResidentSurvivor{
    
    //IComponent_Entity is a component that can be added to any entity
    //it will give the entity a health, speed, and damage
    //it will also remove the entity from the world when it dies
    class IComponent_Entity : SadConsole.Components.UpdateComponent{
        SadConsole.Entities.Entity parent;

        public entityState state = entityState.wandering;

        public int maxHP;
        public int currHP{get; set;}
        int speed;
        public int damage{get; set;}
        public int AC = 10;
        public int AttackBonus = 5;

        public IComponent_Entity(SadConsole.Entities.Entity setParent, int setMaxHP, int setCurrHP, int setSpeed, int setDamage){
            this.parent = setParent;

            maxHP = setMaxHP;
            currHP = setCurrHP;
            speed = setSpeed;
            damage = setDamage;
        }
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            if (currHP < 1){
                //remove from world 
                Game.UIManager.currentFloor.entityManager.Remove(parent);
                //add blood to floor (just a quick little mockup)
                Game.UIManager.currentFloor.addBlood(parent.Position.X, parent.Position.Y,3);
                System.Console.WriteLine(parent.Name + " dead");
            }

            /*
            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
            }
            */
        }
    }
    public enum entityState{
        wandering,
        sleeping,
        hostile,
        friendly
    }
}