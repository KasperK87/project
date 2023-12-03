using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    class Gameover : SadConsole.UI.Window{
        public TimeSpan timer;
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        private uint _score = 0;

            public Gameover(int w, int h, uint highscore) : base( w, h){
                timer = TimeSpan.Zero;

                _score = highscore;
                System.Console.WriteLine("Highscore: " + _score);

                fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
                _selectedFont = fonts[0];

                Position = new Point(0,0);
                DefaultBackground = Color.Black;

                this.Title = "GAME OVER";

            //adding a restart button
            SadConsole.UI.Controls.Button restartButton = new SadConsole.UI.Controls.Button(13,3);
            restartButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            restartButton.Position = new Point(w/2-6,h/2+7);
            restartButton.Text = "RESTART";
            
            restartButton.Click += (x, y) => {
                //Refactor this, should be a method in UIManager
                Game.UIManager.dungeon = new Dungeon(16);
                Game.UIManager.currentFloor = Game.UIManager.dungeon.getCurrentFloor(); 
                Game.Instance.Screen.Children.Add(Game.UIManager.currentFloor);
                Game.Instance.Screen.Children.Add(Game.UIManager.statusScreen);
                Game.Instance.Screen.Children.Add(Game.UIManager.massageScreen);
                Game.UIManager.currentState = ProcessState.Active;
                SadConsole.Game.Instance.Screen.Children.Remove(this);
            };

            //adding a quit button
            SadConsole.UI.Controls.Button quitButton = new SadConsole.UI.Controls.Button(13,3);
            quitButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            quitButton.Position = new Point(w/2-6,h/2+12);
            quitButton.Text = "QUIT ";
            
            quitButton.Click += (x, y) => {
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            
            this.Controls.Add(restartButton);
            this.Controls.Add(quitButton);

            this.Show();
            }

            public override void Update(TimeSpan delta)
            {
                Game.UIManager.pauseScreen.Hide();
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
                
                this.Print(this.Width/2-4, this.Height/2+offcenteringY*2-7,"SCORE: " + _score.ToString(), Color.White, Color.Black);

                base.Render(delta);
                
            }
    }
}