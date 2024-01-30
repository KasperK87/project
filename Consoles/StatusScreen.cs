using SadConsole;
using SadRogue.Primitives;

namespace ResidentSurvivor {
    public class StatusScreen : SadConsole.Console {

        //List of all game objects visaible to the player,
        //ordered by distance from the player
        List<GameObject> gameObjects = new List<GameObject>();
        public StatusScreen(int width, int height) : base(width, height) {
            this.DefaultBackground = SadRogue.Primitives.Color.AnsiCyan;
            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public void addGameObject(GameObject obj){
            gameObjects.Add(obj);
            gameObjects.Sort((x,y) => x.DistanceFromPlayer().CompareTo(y.DistanceFromPlayer()));
        }

        public void removeAllGameObjects(){
            gameObjects.Clear();
        }

        public void drawEntityList(){
            //this.Clear();
            int i = 6;
            foreach(GameObject obj in gameObjects){
                this.Print(1, i, obj.ToString());
                i++;
            }
        }
    }
}