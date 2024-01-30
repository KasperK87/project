using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace ResidentSurvivor {
    public class StatusScreen : SadConsole.Console {

        //List of all game objects visaible to the player,
        //ordered by distance from the player
        public List<GameObject> gameObjects = new List<GameObject>();
        public int _selectedEntity = -1;

        private bool _isDirty = true;
        //always sort for every frame, when = -1
        public ulong currentTurn = 0;
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
            if (currentTurn < Game.UIManager.currentFloor.turn)
                gameObjects.Add(obj);
        }

        public void removeAllGameObjects(){
            gameObjects.Clear();
        }

        public void drawEntityList(){
            //this.Clear();
            if (currentTurn < Game.UIManager.currentFloor.turn)
                gameObjects.Sort((x,y) => x.DistanceFromPlayer().CompareTo(y.DistanceFromPlayer()));
            _isDirty = false;
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
            if (currentTurn < Game.UIManager.currentFloor.turn){
                removeAllGameObjects();
                //currentTurn = Game.UIManager.currentFloor.turn;
            }
        }
    }
}