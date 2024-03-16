using Newtonsoft.Json;
using ResidentSurvivor;
using Xunit;
using Xunit.Sdk;

[Collection("Sequential")]
public class FloorTest{
    public FloorTest(){
        if (ResidentSurvivor.Game.Instance == null){
            ResidentSurvivor.Game.Setup(120, 40);
            ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
        }
    }
    [Fact]
    public void TestFloor(){
        Floor floor = new Floor(80,29);
        Assert.True(floor != null);
    }

    [Fact]
    public void TestJungleStairs(){
        JungleFloor floor = new JungleFloor(80,29);
        Assert.True(testStairs(floor));
    }

    [Fact]
    public void TestDefaultStairs(){
        Floor floor = new Floor(80,29);
        Assert.True(testStairs(floor));
    }
    public bool testStairs(Floor floor){
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
            stairs[1].Position.X, stairs[1].Position.Y) != null){
            return true;
        } else {
            return false;
        }
        
    }

}