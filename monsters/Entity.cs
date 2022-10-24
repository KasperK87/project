//gameObejct extends entity, and is based on a component patten
namespace ResidentSurvivor {
    public class GameObject : SadConsole.Entities.Entity {
        public int currHP;
        public int maxHP;
        protected int speed;

        protected int damage;
        

        public GameObject(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              this.maxHP = 10;
              this.currHP = 10;
              this.speed = 1;
              this.damage = 3;

              this.SadComponents.Add(new IComponent_updater());
              //his.SadComponents.Add(new IComponent_RenderPig());
              
              //crashes many places?!
              //this.SadComponents.Add(new SadConsole.Components.SmoothMove(new SadRogue.Primitives.Point(15,24)));
            }
        
        //Render override is not running, but glyph is still being rendered?
        //renderer appearnly does this behind the scenes
        public override void Render(TimeSpan delta){
            //this.ComponentsRender[0].Render(this, delta);
            //System.Console.WriteLine("pig!");
            //this.Appearance.Glyph = 100;
        }

        public override void Update(TimeSpan delta){
            if (!Game.UIManager.newWorld.DungeonMap.IsInFov(this.Position.X, this.Position.Y)){
                //this.Appearance.Glyph = 0;
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else {
                this.Appearance.Foreground = SadRogue.Primitives.Color.White;
            }
    
            //This allows us to use the render function,
            //but it shouldn't be needed
            foreach (SadConsole.Components.IComponent obj in this.SadComponents){
                obj.Update(this, delta);
                obj.Render(this, delta);
            }

            if (currHP < 1){
                this.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;   
            }

        }

        public void Attack(GameObject target){
            target.currHP -= damage;
        }
    }

    //this works, but can't set glyph
    class IComponent_updater : SadConsole.Components.UpdateComponent {
        public int HP;

        public IComponent_updater(){
            HP = 5;
        }
        public override void Update(SadConsole.IScreenObject parent, TimeSpan delta){
            //System.Console.WriteLine("pig pig!");   
        }
    }

    //this does not get rendered
    class IComponent_RenderPig : SadConsole.Components.RenderComponent {

        public IComponent_RenderPig(){

        }
        public override void Render(SadConsole.IScreenObject parent, TimeSpan delta){
            System.Console.WriteLine("pig!");
        }
    }

}

