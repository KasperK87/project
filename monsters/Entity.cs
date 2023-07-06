//gameObejct extends entity, and is based on a component patten
namespace ResidentSurvivor {
    public class GameObject : SadConsole.Entities.Entity {
        public bool isPlayer = false;
        public int currHP;
        public int maxHP;
        protected int speed;
        public int damage; 
        public Boolean Walkable { get; set; }

        public GameObject(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 10;
              this.currHP = 10;
              this.speed = 1;
              this.damage = 3;
              this.Walkable = true;

              //crashes many places?!
              //this.SadComponents.Add(new SadConsole.Components.SmoothMove(new SadRogue.Primitives.Point(15,24)));
            }
        
        //Render override is not running, but glyph is still being rendered?
        //renderer appearnly does this behind the scenes
        public override void Render(TimeSpan delta){
            //this.ComponentsRender[0].Render(this, delta);
            //System.Console.WriteLine("pig!");
            //this.Appearance.Glyph = 100;

            base.Render(delta);
        }

        //the following code only effects doors, but not 
        //monsters, for some reason... should figure out 
        //why
        public override void Update(TimeSpan delta){
            if (!Game.UIManager.newWorld.GetDungeonMap().IsInFov(this.Position.X, this.Position.Y) && 
                !Game.UIManager.newWorld.GetDungeonMap().IsExplored(this.Position.X, this.Position.Y)){
                //this.Appearance.Glyph = 0;
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                this.Appearance.Foreground = SadRogue.Primitives.Color.White;
            }

            if (currHP < 1){
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;   
            }

            //hooks up the component render functions
            base.Update(delta);

        }

        public virtual void Interact(){
            
        }

        public void Attack(SadConsole.Entities.Entity target){
            target.GetSadComponent<IComponent_Entity>().currHP -= this.damage;
            
        }
    }
}

