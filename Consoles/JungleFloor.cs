namespace ResidentSurvivor{
    class JungleFloor : Floor {
        //list of enemies on the floor
        //list of items on the floor
        //list of special tiles on the floor

        //dungeon floor constructor
        public JungleFloor(int width, int height): base(width, height, true){
            //initializes the floors, basic properties
            initFloor();
        }

        private void initFloor(){
             //sets current turn:
            turn = 0;

            entityManager = new SadConsole.Entities.Manager();
            SadComponents.Add(entityManager);

            //referenced in playerController
            timer = TimeSpan.Zero;

            // Setup this console to accept keyboard input.
            UseKeyboard = true;
            IsVisible = true;
            UseMouse = true;

            // Load the font.
            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            this.Font = fontMaster;
        }
    }
}