/*using RogueSharp;
using SadConsole;
using Console = SadConsole.Console;

namespace RogueSharpSadConsoleSamples.Core
{
   public class Actor
   {
      public Actor()
      {

      }

      // IDrawable
      public Color Color { get; set; }
      public char Symbol { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
      public void Draw( Console mapConsole, IMap map )
      {
         if ( !map.GetCell( X, Y ).IsExplored )
         {
            return;
         }

         if ( map.IsInFov( X, Y ) )
         {
            mapConsole.CellData.SetCharacter( X, Y, Symbol, Color, Colors.FloorBackgroundFov );
         }
         else
         {
            mapConsole.CellData.SetCharacter( X, Y, '.', Colors.Floor, Colors.FloorBackground );
         }
      }
   }
}
*/