//gameObejct extends entity, and is based on a component patten
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;

namespace ResidentSurvivor {
    public class GameObject : SadConsole.Entities.Entity {
        public bool isPlayer = false;
        public string type = "GameObject";
        public int currHP = 1;
        public int maxHP = 1;
        protected int speed = 1;
        public int damage = 1; 
        public Boolean Walkable { get; set; }
        public SadRogue.Primitives.Color color;
        public SadConsole.ColoredString.ColoredGlyphEffect[] frames = new SadConsole.ColoredString.ColoredGlyphEffect[4];
        public TimeSpan timer = TimeSpan.FromMilliseconds(101);

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
            timer += delta;
            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(this.Position.X, this.Position.Y) && 
                !Game.UIManager.currentFloor.GetDungeonMap().IsExplored(this.Position.X, this.Position.Y)){
                //this.Appearance.Glyph = 0;
                //this.setFramesColor(SadRogue.Primitives.Color.Transparent);
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else if (timer > TimeSpan.FromMilliseconds(100)) {
                if (frames[0] != null){
                    this.setFramesColor(this.color);
                }
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

        public void Attack(GameObject target){
            target.GetSadComponent<IComponent_Entity>().currHP -= this.damage;
            target.setFramesColor(SadRogue.Primitives.Color.Red);
            target.timer = TimeSpan.Zero;
        }

        public void setFramesColor(SadRogue.Primitives.Color c){
            if (frames[0] == null){
                return;
            }
            foreach (SadConsole.ColoredString.ColoredGlyphEffect frame in frames){
                frame.Foreground = c;
            }
        }
    }
}

