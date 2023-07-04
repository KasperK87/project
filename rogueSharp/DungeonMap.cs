using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using SadConsole;
//using SadRogue.Primitives;
using Console = SadConsole.Console;
using TileType = ResidentSurvivor.TileType;

namespace RogueSharpSadConsoleSamples.Core
{
    public class DungeonMap : Map{
        public List<Rectangle> Rooms;
        public List<ResidentSurvivor.Door> Doors;

        public DungeonMap(){
            Rooms = new List<Rectangle>();
            Doors = new List<ResidentSurvivor.Door>();
        }

        /*
        public void AddPlayer( Player player ){
            RogueGame.Player = player;
            SetIsWalkable( player.X, player.Y, false );
            UpdatePlayerFieldOfView();
            RogueGame.SchedulingSystem.Add( player );
        }
        */

        
        public void UpdatePlayerFieldOfView(SadConsole.Entities.Entity? player){
         if (player != null){
            ComputeFov(player.Position.X, player.Position.Y, 10, true );
            foreach ( Cell cell in GetAllCells() )
            {
                if ( IsInFov( cell.X, cell.Y ) )
               {
                    SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true );
               }
            }
         }
        }
      

        /*
      public void SetIsWalkable( int x, int y, bool isWalkable )
      {
         Cell cell = GetCell( x, y );
         SetCellProperties( cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored );
      }
      */

      public void Draw( Console mapConsole)
      {
         //mapConsole.Clear();
         foreach ( Cell cell in GetAllCells() )
         {
            SetConsoleSymbolForCell( mapConsole, cell);
         }
      }

      private void SetConsoleSymbolForCell(SadConsole.Console map, Cell cell)
      {
         if ( !cell.IsExplored )
         {
            return;
         }

         if (IsInFov( cell.X, cell.Y ) )
         {
            if ( cell.IsWalkable )
            {
               if (GetCell(cell.X, cell.Y).IsWalkable && !GetCell(cell.X, cell.Y-1).IsWalkable)
                    map.SetGlyph(cell.X, cell.Y-1, (int) TileType.Wall);
               /*
               if (!map.GetGlyph(cell.X, cell.Y-1).Equals((int) TileType.Floor))
                    map.SetGlyph(cell.X, cell.Y-1, (int) TileType.Wall);
               */
               
                map.SetGlyph(cell.X, cell.Y, (int) TileType.Floor);
               
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Blue);  
              
                //console.CellData.SetCharacter( cell.X, cell.Y, '.', Colors.FloorFov, Colors.FloorBackgroundFov );
            }
            else
            {

                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Red);
                map.SetGlyph(cell.X, cell.Y, (int) TileType.Solid);
            }
         }
         else
         {      
            if ( cell.IsWalkable )
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);
                //map.SetForeground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);
               //console.CellData.SetCharacter( cell.X, cell.Y, '.', Colors.Floor, Colors.FloorBackground );
            }
            else
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Brown);
               //console.CellData.SetCharacter( cell.X, cell.Y, '#', Colors.Wall, Colors.WallBackground );
            }
         }
      }

    }
}