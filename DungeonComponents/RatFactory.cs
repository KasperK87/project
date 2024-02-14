using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    public class RatFactory : GameObjectFactory{
        private int _weight = 1;

        public RatFactory(int weight){
            this._weight = weight;
        }

        public override GameObject create(Floor parent, Point cell){
            GameObject rat = new GameObject(Color.Gray, Color.Transparent, (int) TileType.Rat, 99);
                        
            HelperFunctionsEntities.createAnimation(rat,(int)TileType.Rat);

            rat.Name = "Rat";

            rat.Position = new Point(cell.X,cell.Y);
            rat.Walkable = false;

            var entity = new IComponent_Entity(rat, 1, 1, 1, 1);

            rat.SadComponents.Add(entity);
            rat.SadComponents.Add(new IComponent_Hostile(rat, entity, parent));
            return rat;
        }

        public override int getWeight(){
            return this._weight;
        }
    }
}