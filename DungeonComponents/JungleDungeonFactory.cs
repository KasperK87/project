namespace ResidentSurvivor{
    public class JungleDungeonFactory{
        public static RogueSharpSadConsoleSamples.Core.DungeonMap createJungleDungeon(){
            //Temporarily using RogueSharp's RandomRoomsMapCreationStrategy
            RogueSharp.MapCreation.IMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap> mapCreationStrategy =
                new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharpSadConsoleSamples.Core.DungeonMap>( 80, 29, 100, 7, 3 );
            
            RogueSharpSadConsoleSamples.Core.DungeonMap jungle;
            
            jungle = mapCreationStrategy.CreateMap();
            
            return jungle;
        }
    }
}