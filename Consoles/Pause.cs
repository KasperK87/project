using SadConsole.Input;

namespace   ResidentSurvivor {

    public class Pause : SadConsole.UI.Window {
        public TimeSpan timer;
        private SadConsole.Readers.TheDrawFont[] fonts;
        private SadConsole.Readers.TheDrawFont _selectedFont;

        private uint _score = 0;

        public Pause(int w, int h, uint highscore) : base( w, h){
            _score = highscore;

            fonts = SadConsole.Readers.TheDrawFont.ReadFonts("./fonts/TheDraw/ABBADON.TDF").ToArray();
            _selectedFont = fonts[0];

            Position = new SadRogue.Primitives.Point(0,0);
            DefaultBackground = SadRogue.Primitives.Color.Black;

            this.Title = "PAUSED";

            timer = TimeSpan.Zero;

            //adding a restart button
            SadConsole.UI.Controls.Button restartButton = new SadConsole.UI.Controls.Button(13,3);
            restartButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            restartButton.Position = new SadRogue.Primitives.Point(w/2-6,h/2+9-6);
            restartButton.Text = "RESTART";
            
            restartButton.Click += (x, y) => {
                //Refactor this, should be a method in UIManager
                Game.UIManager.dungeon = new Dungeon(16);
                
                Game.Instance.Screen.Children.Remove(Game.UIManager.currentFloor);
                Game.Instance.Screen.Children.Remove(Game.UIManager.statusScreen);
                Game.Instance.Screen.Children.Remove(Game.UIManager.massageScreen);

                Game.UIManager.currentFloor = Game.UIManager.dungeon.getCurrentFloor(); 

                Game.Instance.Screen.Children.Add(Game.UIManager.currentFloor);
                Game.Instance.Screen.Children.Add(Game.UIManager.statusScreen);
                Game.Instance.Screen.Children.Add(Game.UIManager.massageScreen);
                Game.UIManager.currentState = ProcessState.Active;
                this.IsFocused = false;
                this.Hide();
            };

            //adding a continue button
            SadConsole.UI.Controls.Button continueButton = new SadConsole.UI.Controls.Button(13,3);
            continueButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();

            continueButton.Position = new SadRogue.Primitives.Point(w/2-6,h/2+4-6);
            continueButton.Text = "CONTINUE";

            continueButton.Click += (x, y) => {
                Game.UIManager.currentState = ProcessState.Active;
                this.IsFocused = false;
                this.Hide();
            };

            //adding a quit button
            SadConsole.UI.Controls.Button quitButton = new SadConsole.UI.Controls.Button(13,3);
            quitButton.Theme = new SadConsole.UI.Themes.ButtonLinesTheme();
            
            quitButton.Position = new SadRogue.Primitives.Point(w/2-6,h/2+12-6);
            quitButton.Text = "QUIT ";
            
            quitButton.Click += (x, y) => {
                SadConsole.Game.Instance.MonoGameInstance.Exit();
            };
            
            this.Controls.Add(continueButton);
            this.Controls.Add(restartButton);
            this.Controls.Add(quitButton);

            this.Show();
        }

        public override void Show(bool modal)
        {
            timer = TimeSpan.Zero;
            base.Show(modal);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyReleased(SadConsole.Input.Keys.Escape) && timer.TotalMilliseconds > 500){
                    Game.UIManager.currentState = ProcessState.Active;
                    this.IsFocused = false;
                    this.Hide();
                }  
            return base.ProcessKeyboard(info);
        }

        public override void Update(System.TimeSpan delta){
            timer += delta;
            base.Update(delta);
        }
    }

}