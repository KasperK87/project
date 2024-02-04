using Newtonsoft.Json;
using ResidentSurvivor;

class FloorTest{
    public static void testStairs(Floor floor){
        //Floor floor = new Floor(80,29);

        // arrange
        List<GameObject> stairs = new List<GameObject>();

        IReadOnlyList<SadConsole.Entities.Entity> list = floor.GetEntities();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = list[i] as GameObject;
            if (obj.type == "Stairs"){
                stairs.Add(obj);
            }
        }

        if (stairs != null && stairs.Count != 2){
            Console.WriteLine("There should be 2 stairs on the floor");
        } else {
            Console.WriteLine("There are 2 stairs on the floor");
        }
        
        if (stairs.Count == 2 && floor.pathBetween(stairs[0].Position.X, stairs[0].Position.Y, 
            stairs[0].Position.X, stairs[0].Position.Y) == null){
            Console.WriteLine("There should be a path between the stairs");
        } else {
            Console.WriteLine("There is a path between the stairs");
        }
        
    }

}