using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using SadConsole;
//using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace RogueSharpSadConsoleSamples.Core
{
    public class DungeonMap : Map{
        public List<Rectangle> Rooms;

        public DungeonMap(){
            Rooms = new List<Rectangle>();
        }

        /*
        public void AddPlayer( Player player ){
            RogueGame.Player = player;
            SetIsWalkable( player.X, player.Y, false );
            UpdatePlayerFieldOfView();
            RogueGame.SchedulingSystem.Add( player );
        }
        */

        /*
        public void UpdatePlayerFieldOfView(){
         Player player = RogueGame.Player;
         ComputeFov( player.X, player.Y, player.Awareness, true );
         foreach ( Cell cell in GetAllCells() )
         {
            if ( IsInFov( cell.X, cell.Y ) )
            {
               SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true );
            }
         }
      }
      */

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
            SetConsoleSymbolForCell( mapConsole, cell );
         }
      }

      private void SetConsoleSymbolForCell(SadConsole.Console map, Cell cell )
      {
         if ( !cell.IsExplored )
         {
            //return;
         }

         if ( IsInFov( cell.X, cell.Y ) )
         {
            if ( cell.IsWalkable )
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.White);
                
               //console.CellData.SetCharacter( cell.X, cell.Y, '.', Colors.FloorFov, Colors.FloorBackgroundFov );
            }
            else
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);
               //console.CellData.SetCharacter( cell.X, cell.Y, '#', Colors.WallFov, Colors.WallBackgroundFov );
            }
         }
         else
         {
            if ( cell.IsWalkable )
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.White);
               //console.CellData.SetCharacter( cell.X, cell.Y, '.', Colors.Floor, Colors.FloorBackground );
            }
            else
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);
               //console.CellData.SetCharacter( cell.X, cell.Y, '#', Colors.Wall, Colors.WallBackground );
            }
         }
      }

    }
}