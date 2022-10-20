//gameObejct extends entity, and is based on a component patten
namespace ResidentSurvivor {
    class GameObject : SadConsole.Entities.Entity {

        public GameObject(SadRogue.Primitives.Color c1,SadRogue.Primitives.Color c2, int SetGlyph, int zIndex):
            base(c1, c2, SetGlyph, zIndex){

            }

        public override void Update(TimeSpan delta){
            this.Appearance.Glyph = 100;
            base.Update(delta);
        }
    }
}