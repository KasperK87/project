using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    class Gameover : SadConsole.UI.Window{
        public TimeSpan timer;
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

            public Gameover(int w, int h) : base( w, h){
                timer = TimeSpan.Zero;

                fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
                _selectedFont = fonts[0];

                Position = new Point(0,0);
                DefaultBackground = Color.Black;

                this.Show();
                
            }

            public override void Render(TimeSpan delta)
            {
                this.Clear();
                //timer += delta;

                int offcenteringX =30;
                int offcenteringY = (int)(timer.TotalMilliseconds/300);
                Surface.PrintTheDraw(2+offcenteringX, this.Height/2-10-offcenteringY, "GAME", _selectedFont);
                Surface.PrintTheDraw(this.Width/2, this.Height/2-offcenteringY, "OVER", _selectedFont);

                base.Render(delta);
            }
    }
}