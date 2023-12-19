namespace ResidentSurvivor {
    public class StatusScreen : SadConsole.Console {
        public StatusScreen(int width, int height) : base(width, height) {
            this.DefaultBackground = SadRogue.Primitives.Color.AnsiCyan;
            this.Position = new SadRogue.Primitives.Point(1, 1);
        }
    }
}