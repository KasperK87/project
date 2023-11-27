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

                this.Title = "GAME OVER";

                 //adding a start button
            SadConsole.UI.Controls.Button restartButton = new SadConsole.UI.Controls.Button(13,3);
            //startButton.Theme = new SadConsole.UI.Themes.ButtonTheme('[',']');
            restartButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            //NOT VALID WHY?
            //startButton.SetThemeColors((SadConsole.UI.Colors.ColorNames.Orange);
            
            restartButton.Position = new Point(w/2-6,h/2+6);
            restartButton.Text = "RESTART";
            
            restartButton.Click += (x, y) => {
                
                // World is created in UIManeger 
                // and will appears when we hide 
                // this menu;

                // This code is needed if we change 
                // how world is created

                restartButton.Text = "CLICKED";

                //hides the menu
                //this.Hide();
            };
            
            
            
            this.Controls.Add(restartButton);

            this.Show();
                
            }

            public override void Update(TimeSpan delta)
            {
                base.Update(delta);
            }

            public override void Render(TimeSpan delta)
            {
                
                //this.Clear();
                //timer += delta;

                
                int offcenteringX = 50;
                int offcenteringY = (int)(timer.TotalMilliseconds/300)+6;
                Surface.PrintTheDraw(2+offcenteringX-20, this.Height/2-10-offcenteringY, "GAME", _selectedFont);
                Surface.PrintTheDraw(this.Width/2-20, this.Height/2-offcenteringY, "OVER", _selectedFont);
                
                base.Render(delta);
                
            }
    }
}