using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;


namespace ResidentSurvivor{
    class RandomTable{
        private List<GameObjectFactory> table = new List<GameObjectFactory>();

        private RandomTable(){
        }

        public static RandomTable jungle(){
            RandomTable table = new RandomTable();
            table.add(new RatFactory(1));
            return table;
        }

        public void add(GameObjectFactory obj){
            table.Add(obj);
        }

        public GameObject roll(Floor parent, Point cell){
            int totalWeight = 0;
            foreach(GameObjectFactory obj in table){
                totalWeight += obj.getWeight();
            }
            //a little messy with a static reference
            //to Floor.Random.  I'll fix this later
            int roll = Floor.Random.Next(totalWeight);
            int currentWeight = 0;
            foreach(GameObjectFactory factory in table){
                currentWeight += factory.getWeight();
                if(roll < currentWeight){
                    return factory.create(parent, cell);
                }
            }
            return null;
        }
    }
}