//gameObejct extends entity, and is based on a component patten
using System.Runtime.ConstrainedExecution;

namespace ResidentSurvivor {
    public class GameObject : SadConsole.Entities.Entity {
        public bool isPlayer = false;
        public string type = "GameObject";
        public int currHP;
        public int maxHP;
        protected int speed;
        public int damage; 
        public Boolean Walkable { get; set; }

        private SadRogue.Primitives.Color color;

        public GameObject(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 10;
              this.currHP = 10;
              this.speed = 1;
              this.damage = 3;
              this.Walkable = true;

              this.color = c1;

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

        public override void Update(TimeSpan delta){
            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(this.Position.X, this.Position.Y) && 
                !Game.UIManager.currentFloor.GetDungeonMap().IsExplored(this.Position.X, this.Position.Y)){
                //this.Appearance.Glyph = 0;
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                this.Appearance.Foreground = this.color;
            }
    
            //This allows us to use the render function,
            //but it shouldn't be needed
            //for some reason render component are not called.
            /*
            foreach (SadConsole.Components.IComponent obj in this.SadComponents){
                obj.Update(this, delta);
                //obj.Render(this, delta);
            }
            */

            if (currHP < 1){
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;   
            }

            //hooks up the component render functions
            base.Update(delta);

        }

        public virtual bool Interact(){
            return false;
        }

        //used for items, intend is only the player can pickup
        public virtual void Pickup(Player player){
            
        }

        public void Attack(SadConsole.Entities.Entity target){
            target.GetSadComponent<IComponent_Entity>().currHP -= this.damage;
            
        }
    }
}

