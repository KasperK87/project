using SadConsole;
using SadConsole.Input;

namespace ResidentSurvivor {
    public class Inventory :  SadConsole.UI.Window{
        //player has the inventory
        //this class is the GUI for the inventory
        private Player player;
        private TimeSpan timer;
        public Inventory(Player pPlayer) : base(60, 20){
            player = pPlayer;

            Position = new SadRogue.Primitives.Point(60-30, 20-10);
            DefaultBackground = SadRogue.Primitives.Color.Black;
            this.Title = "INVENTORY";

            timer = TimeSpan.Zero;
        }

        public override void Render(TimeSpan delta)
        {
            //this.Clear();

            //show inventory
            for (int i = 0; i < player.getInventory().Length; i++)
            {
                //clears the line
                this.Clear(1, i+1, 10);
                //prints the item
                this.Print(1, i+1, player.getInventory()[i], 
                    SadRogue.Primitives.Color.White, 
                    SadRogue.Primitives.Color.Black);
            }

            base.Render(delta);
        }
         public override void Show(bool modal)
        {
            timer = TimeSpan.Zero;
            base.Show(modal);
        }
        public override bool ProcessKeyboard(Keyboard info)
        {
            if ((info.IsKeyReleased(SadConsole.Input.Keys.Escape) || info.IsKeyReleased(SadConsole.Input.Keys.I)) &&
                 timer.TotalMilliseconds > 500){
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