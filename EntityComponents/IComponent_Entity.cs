namespace ResidentSurvivor{
    class IComponent_Entity : SadConsole.Components.UpdateComponent{
        GameObject parent;

        int maxHP;
        int currHP;
        int speed;
        int damage;

        public IComponent_Entity(GameObject setParent, int setMaxHP, int setCurrHP, int setSpeed, int setDamage){
            this.parent = setParent;

            maxHP = setMaxHP;
            currHP = setCurrHP;
            speed = setSpeed;
            damage = setDamage;
        }
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            if (!Game.UIManager.newWorld.DungeonMap.IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
            }
        }
    }
}