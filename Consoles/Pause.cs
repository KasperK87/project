namespace   ResidentSurvivor {

    class Pause : SadConsole.UI.Window {
        public TimeSpan timer;
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        private uint _score = 0;

        public Pause(int w, int h, uint highscore) : base( w, h){
            _score = highscore;
            System.Console.WriteLine("Highscore: " + _score);

            fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
            _selectedFont = fonts[0];

            Position = new SadRogue.Primitives.Point(0,0);
            DefaultBackground = SadRogue.Primitives.Color.Black;

            this.Title = "PAUSED";

            //adding a restart button
            SadConsole.UI.Controls.Button restartButton = new SadConsole.UI.Controls.Button(13,3);
            restartButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            restartButton.Position = new SadRogue.Primitives.Point(w/2-6,h/2+7);
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
            
            quitButton.Position = new SadRogue.Primitives.Point(w/2-6,h/2+12);
            quitButton.Text = "QUIT ";
            
            quitButton.Click += (x, y) => {
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            
            this.Controls.Add(restartButton);
            this.Controls.Add(quitButton);

            this.Show();
        }

        public override void Update(System.TimeSpan delta){
            base.Update(delta);
        }
    }

}