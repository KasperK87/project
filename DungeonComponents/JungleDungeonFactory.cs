using RogueSharp;
using DungeonMap = RogueSharpSadConsoleSamples.Core.DungeonMap;

namespace ResidentSurvivor{
    public class JungleDungeonFactory{
        public static DungeonMap createJungleDungeon(){
            DungeonMap jungle;

            jungle = generateFloorLayout(80,29);

            return jungle;
        }

        private static DungeonMap generateFloorLayout(int width, int height){
            DungeonMap floorLayout = new DungeonMap(width,height);

            //makes 11x11 room
            foreach (Cell cell in floorLayout.GetCellsInSquare(20,20,5)){
                floorLayout.SetCellProperties(cell.X, cell.Y, true, true);
            }

            return floorLayout; 
        }
    }    
}