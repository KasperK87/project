using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;


namespace ResidentSurvivor {
    public abstract class GameObjectFactory {
        public abstract GameObject create(Floor parent, Point cell);

        public abstract int getWeight();
    }
}