using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    public class Dungeon{
        private int depth;

        private Level[] levels;
        public int currentLevel;
        public Dungeon(int setDepth){
            depth = setDepth;
            levels = new Level[depth];

            currentLevel = 0;

            for (int i = depth-1; i >= 0; i--){
                levels[i] = new Level(80,29);
                levels[i].Position = new Point(21,1);
                levels[i].DefaultBackground = Color.Black;
                levels[i].View = new Rectangle(0, 0, 20, 15);
            }
        }

        public void setLevel(int setLevel){
            
            //transfer player
            levels[setLevel].GetSadComponent<IComponent_PlayerControls>().parent = levels[currentLevel].getPlayer();

            //levels[setLevel].entityManager.Remove(levels[currentLevel].GetSadComponent<IComponent_PlayerControls>().parent);
            levels[setLevel].entityManager.Remove(levels[currentLevel].getPlayer());
            levels[setLevel].entityManager.Add(levels[setLevel].GetSadComponent<IComponent_PlayerControls>().parent);
            
            
            currentLevel = setLevel;
        }
            
        public Level getCurrentLevel(){
            return levels[currentLevel];
        }
    }
}
