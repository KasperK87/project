using SadConsole;
using SadConsole.Input;

namespace ResidentSurvivor {
    public class Inventory :  SadConsole.UI.Window{
        //player has the inventory
        //this class is the GUI for the inventory
        private Player player;
        private TimeSpan timer;

        private int _selectedItem;
        public Inventory(Player pPlayer) : base(60, 20){
            player = pPlayer;

            Position = new SadRogue.Primitives.Point(60-30, 20-10);
            DefaultBackground = SadRogue.Primitives.Color.Black;
            this.Title = "INVENTORY";

            timer = TimeSpan.Zero;
            _selectedItem = 0;
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
                if (i == _selectedItem)
                    this.Print(1, i+1, player.getInventory()[i], 
                        SadRogue.Primitives.Color.White, 
                        SadRogue.Primitives.Color.Red);
                else {
                    this.Print(1, i+1, player.getInventory()[i], 
                        SadRogue.Primitives.Color.White, 
                        SadRogue.Primitives.Color.Black);
                }
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
            if (info.IsKeyPressed(SadConsole.Input.Keys.Down) && timer.TotalMilliseconds > 200){
                _selectedItem++;
                if (_selectedItem >= player.getInventory().Length)
                    _selectedItem = 0;
                timer = TimeSpan.Zero;
            } else if (info.IsKeyPressed(SadConsole.Input.Keys.Up)){
                _selectedItem--;
                if (_selectedItem < 0)
                    _selectedItem = player.getInventory().Length-1;
                timer = TimeSpan.Zero;
            } else if (info.IsKeyPressed(SadConsole.Input.Keys.Enter)){
                if (player.getInventory()[_selectedItem] != null){
                    player.equipItem(_selectedItem);
                    //should this take a turn?
                    closeWindow();
                }
            } else if ((info.IsKeyReleased(SadConsole.Input.Keys.Escape) || info.IsKeyReleased(SadConsole.Input.Keys.I)) &&
                 timer.TotalMilliseconds > 200){
                    closeWindow();
                }  
            return base.ProcessKeyboard(info);
        }
        private void closeWindow(){
            Game.UIManager.currentState = ProcessState.Active;
            this.IsFocused = false;
            this.Hide();
        }
        public override void Update(System.TimeSpan delta){
            timer += delta;
            base.Update(delta);
        }
    }
}