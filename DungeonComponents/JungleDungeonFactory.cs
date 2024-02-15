using RogueSharp;

namespace ResidentSurvivor{
    public class JungleDungeonFactory{
        public static RogueSharpSadConsoleSamples.Core.DungeonMap createJungleDungeon(){
            //Temporarily using RogueSharp's RandomRoomsMapCreationStrategy
            //RogueSharp.MapCreation.IMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap> mapCreationStrategy =
            //    new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap>( 80, 29, 100, 7, 3 );
            
            RogueSharpSadConsoleSamples.Core.DungeonMap jungle;

            jungle = new RogueSharpSadConsoleSamples.Core.DungeonMap(80,29);
            
            //make a squere room
            foreach (Cell cell in jungle.GetCellsInSquare(20,20,5)){
                jungle.SetCellProperties(cell.X, cell.Y, true, true);
            }

            //jungle = mapCreationStrategy.CreateMap();
            
            return jungle;
        }
    }
}