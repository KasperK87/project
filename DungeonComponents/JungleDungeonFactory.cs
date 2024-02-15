using RogueSharp;

namespace ResidentSurvivor{
    public class JungleDungeonFactory{
        public static RogueSharpSadConsoleSamples.Core.DungeonMap createJungleDungeon(){
            RogueSharpSadConsoleSamples.Core.DungeonMap jungle;

            jungle = new RogueSharpSadConsoleSamples.Core.DungeonMap(80,29);
            
            //makes 11x11 room
            foreach (Cell cell in jungle.GetCellsInSquare(20,20,5)){
                jungle.SetCellProperties(cell.X, cell.Y, true, true);
            }

            return jungle;
        }
    }
}