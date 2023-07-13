using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    public class Dungeon{
        private int depth;

        private Floor[] floors;
        private int currentLevel;
        public Dungeon(int setDepth){
            depth = setDepth;
            floors = new Floor[depth];

            currentLevel = 0;

            for (int i = depth-1; i >= 0; i--){
                floors[i] = new Floor(80,29);
                floors[i].Position = new Point(21,1);
                floors[i].DefaultBackground = Color.Black;
                floors[i].View = new Rectangle(0, 0, 20, 15);
            }
        }

        public void setLevel(int setLevel){
            if (setLevel >= 0 && setLevel <= depth){
                //transfer player
                floors[setLevel].playerEnterFromUp(
                    (GameObject)floors[currentLevel].GetSadComponent<IComponent_PlayerControls>().parent,
                    currentLevel < setLevel ? true : false
                );
        
                currentLevel = setLevel;
            } else {
                Game.UIManager.currentState = ProcessState.Terminated;
            }
        }
            
        public Floor getCurrentFloor(){
            return floors[currentLevel];
        }

        public int getCurrentLevel(){
            return currentLevel;
        }
    }
}
