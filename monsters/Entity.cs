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
        private Random rand = new Random();

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
            int roll = (int)rand.Next(1,20);
            if (roll == 20){
                target.hit(this.GetSadComponent<IComponent_Entity>().damage, 
                    new damageModifier[]{damageModifier.critical});
                System.Console.WriteLine(roll + ": The " + this.Name + " crit the " + target.Name);
                //Game.UIManager.messageLog.Add("The " + this.Name + " crit the " + target.Name);
                return;
            }
            roll += this.GetSadComponent<IComponent_Entity>().AttackBonus;
            if (roll >= target.GetSadComponent<IComponent_Entity>().AC){
                target.hit(this.GetSadComponent<IComponent_Entity>().damage);
                System.Console.WriteLine(roll + ": The " + this.Name + " hit the " + target.Name);
            } else {
                System.Console.WriteLine(roll+ ": The " + this.Name + " missed the " + target.Name);
                //Game.UIManager.messageLog.Add("The " + this.Name + " missed the " + target.Name);
            }
        }

        //when the entitie is hit
        public void hit(int damage, damageModifier[] mods = null){
            if (mods != null)
            for (int i = 0; i < mods.Length; i++){
                switch (mods[i]){
                    case damageModifier.critical:
                        damage *= 2;
                        break;
                    case damageModifier.fire:
                        break;
                    case damageModifier.ice:
                        break;
                    case damageModifier.poison:
                        break;
                    case damageModifier.acid:
                        break;
                    case damageModifier.lightning:
                        break;
                    case damageModifier.holy:
                        break;
                    case damageModifier.unholy:
                        break;
                }
            }
            this.GetSadComponent<IComponent_Entity>().currHP -= damage;

            this.setFramesColor(SadRogue.Primitives.Color.Red);
            this.Appearance.Foreground = SadRogue.Primitives.Color.Red;
            this.timer = TimeSpan.Zero;

            //add blood to floor (just a quick little mockup)
            Game.UIManager.currentFloor.addBlood(Position.X, Position.Y,1);
            
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
    public enum damageModifier{
        none,
        critical,
        fire,
        ice,
        poison,
        acid,
        lightning,
        holy,
        unholy
    }
}

