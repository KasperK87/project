//gameObejct extends entity, and is based on a component patten
namespace ResidentSurvivor {
    class GameObject : SadConsole.Entities.Entity {
        protected int currHP;
        protected int maxHP;
        protected int speed;
        

        public GameObject(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){
              //IComponent_updater pig = new IComponent_updater();
              //this.ComponentsUpdate.Add(pig);
            }
        
        //Render override is not running, but glyph is still being rendered?
        public override void Render(TimeSpan delta){
            //this.ComponentsRender[0].Render(this, delta);
            //System.Console.WriteLine("pig!");
            //this.Appearance.Glyph = 100;
        }

        public override void Update(TimeSpan delta){
            //this.Appearance.Glyph = 100;
            
            //This allows us to use the render function,
            //but it shouldn't be needed
            //this.Render(delta);
            
            base.Update(delta);

        }
    }

    //this works, but can't set glyph
    class IComponent_updater : SadConsole.Components.UpdateComponent {
        public override void Update(SadConsole.IScreenObject parent, TimeSpan delta){
            //System.Console.WriteLine("pig!");   
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

