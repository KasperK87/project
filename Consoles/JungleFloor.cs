using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace ResidentSurvivor{
    class JungleFloor : Floor {
        //list of enemies on the floor
        private RandomTable monsterTable;
        //list of items on the floor
        //list of special tiles on the floor

        //dungeon floor constructor
        public JungleFloor(int width, int height): base(width, height, true){
            //initializes the floors, basic properties
            initFloor();

            monsterTable = RandomTable.jungle();

            generateDungeon();

            decorateDungeon();
        }

        private void initFloor(){
             //sets current turn:
            turn = 0;

            entityManager = new SadConsole.Entities.Manager();
            SadComponents.Add(entityManager);
            entityManager.Add(player);

            //add the player controls to the floor
            //important for finding the player
            //with the getPlayer method
            SadComponents.Add(new IComponent_PlayerControls(player, this));

            //referenced in playerController
            timer = TimeSpan.Zero;

            // Setup this console to accept keyboard input.
            UseKeyboard = true;
            IsVisible = true;
            UseMouse = true;

            // Load the font.
            var fontMaster = SadConsole.Game.Instance.LoadFont("./fonts/_test.font");
            this.Font = fontMaster;

            //init the player, will be moved when the player enters the floor
            //entityManager.Add(player);
        }
    
        private void generateDungeon(){

            DungeonMap = JungleDungeonFactory.createJungleDungeon();

            tileMetadata = new TileMetadata[80,29];

            for (int x = 1; x < 79; x++){
                for (int y = 1; y < 28; y++){
                    tileMetadata[x,y] = new TileMetadata(0);
                }
            }
        }

        private void decorateDungeon(){
            //decorates the dungeon with enemies, items, and special tiles
            bool placedDownstairs = false;
            bool placedUpstairs = false;

            for (int x = 0; x < 80; x++){
                for (int y = 0; y < 29; y++){
                    if(DungeonMap.IsWalkable(x,y)){

                        if (Floor.Random.Next(10)==0 && !placedUpstairs){
                            DungeonMap.SetCellProperties(x, y, true, true, false);
                            
                            Stairs stairs = new Stairs(
                            Color.White, Color.Transparent, (int) TileType.UpStairs, 98, true);
                        
                            stairs.Position = new Point(x, y);

                            entityManager.Add(stairs);

                            placedUpstairs = true;
                        } else if (Floor.Random.Next(10)==0 && !placedDownstairs){
                            DungeonMap.SetCellProperties(x, y, true, true, false);
                            
                            Stairs stairs = new Stairs(
                            Color.White, Color.Transparent, (int) TileType.DownStairs, 98, false);
                        
                            stairs.Position = new Point(x, y);

                            entityManager.Add(stairs);

                            placedDownstairs = true;
                        } else if (Floor.Random.Next(10) == 0) {

                        }
                        /*
                        GameObject obj = monsterTable.roll(this, new Point(x,y));
                        if(obj != null){
                            entityManager.Entities.Add(obj);
                        }
                        */
                    }
                }
            }
        }
    }
}