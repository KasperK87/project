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

      //dungeon color theme
        public SadRogue.Primitives.Color cWall = SadRogue.Primitives.Color.DarkBlue;
        public SadRogue.Primitives.Color cFloor = SadRogue.Primitives.Color.Black;
        
        public List<Rectangle> Rooms;
        public List<ResidentSurvivor.Door> Doors;

        //creates a boring map of solid stone 
        public DungeonMap(int width, int height) : base (width, height){
            Rooms = new List<Rectangle>();
            Doors = new List<ResidentSurvivor.Door>();
        }

        public DungeonMap(){
            Rooms = new List<Rectangle>();
            Doors = new List<ResidentSurvivor.Door>();
        }

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
         if ( !cell.IsExplored ){return;}

         if (IsInFov( cell.X, cell.Y ) )
         {
            if ( cell.IsWalkable )
            {
               //TODO: should be done in generate map
               if (GetCell(cell.X, cell.Y).IsWalkable && !GetCell(cell.X, cell.Y-1).IsWalkable){
                  map.SetGlyph(cell.X, cell.Y-1, (int) TileType.Wall);
                  map.SetBackground(cell.X,cell.Y-1, cWall);
               }

               map.SetGlyph(cell.X, cell.Y, (int) TileType.Floor);
               map.SetBackground(cell.X, cell.Y, cFloor);  
            }
            else
            {
               //TODO: should be done in generate map
               map.SetBackground(cell.X, cell.Y, cWall);
               if (cell.Y+1 < map.Height)
                  if (GetCell(cell.X, cell.Y+1).IsWalkable && !GetCell(cell.X, cell.Y).IsWalkable)
                     map.SetGlyph(cell.X, cell.Y, (int) TileType.Wall);
                  else
                     map.SetGlyph(cell.X, cell.Y, (int) TileType.Solid);
               else
                  map.SetGlyph(cell.X, cell.Y, (int) TileType.Solid);
               
            }
         }
         else
         {      
            if ( cell.IsWalkable )
            {
               map.SetGlyph(cell.X, cell.Y, (int) TileType.Floor);
               map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);  
            }else
            {
                map.SetBackground(cell.X, cell.Y, SadRogue.Primitives.Color.Gray);
                //TODO: should be done in generate map
                if (cell.Y+1 < map.Height)
                  if (GetCell(cell.X, cell.Y+1).IsWalkable && !GetCell(cell.X, cell.Y).IsWalkable)
                     map.SetGlyph(cell.X, cell.Y, (int) TileType.Wall);
                  else
                     map.SetGlyph(cell.X, cell.Y, (int) TileType.Solid);
                else
                  map.SetGlyph(cell.X, cell.Y, (int) TileType.Solid);
            }
         }
      }
   }
}