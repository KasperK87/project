using Xunit;

namespace ResidentSurvivorTest{

    public class MonoGameFixture : IDisposable{
        public MonoGameFixture(){
            if (ResidentSurvivor.Game.Instance == null){
                ResidentSurvivor.Game.Setup(120, 40);
                ResidentSurvivor.Game.Instance.MonoGameInstance.RunOneFrame();
            }
        }

        public void Dispose(){
            ResidentSurvivor.Game.Instance.MonoGameInstance.Exit();
        }
    }

    [CollectionDefinition("Sequential")]
    public class MonoGameFixtureCollection : ICollectionFixture<MonoGameFixture>{
    }
}