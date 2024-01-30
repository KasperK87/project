using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace ResidentSurvivor {
    public class StatusScreen : SadConsole.Console {

        //List of all game objects visaible to the player,
        //ordered by distance from the player
        public List<GameObject> gameObjects = new List<GameObject>();
        public int _selectedEntity = -1;
        public StatusScreen(int width, int height) : base(width, height) {
            this.DefaultBackground = SadRogue.Primitives.Color.AnsiCyan;
            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public override bool ProcessMouse(MouseScreenObjectState state){
            if (state.CellPosition.X >= 1 && state.CellPosition.X <= 20 &&
                state.CellPosition.Y >= 6 && state.CellPosition.Y <= gameObjects.Count+5){
                    _selectedEntity = state.CellPosition.Y;
            } else {
                _selectedEntity = -1;
            }         
            return false;
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
                if (i == _selectedEntity)
                    this.Print(1, i, obj.ToString(), 
                        SadRogue.Primitives.Color.White, 
                        SadRogue.Primitives.Color.Red);
                else {
                    this.Print(1, i, obj.ToString(), 
                        SadRogue.Primitives.Color.White, 
                        SadRogue.Primitives.Color.AnsiCyan);
                }
                i++;
            }
        }
    }
}