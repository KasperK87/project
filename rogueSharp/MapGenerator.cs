using System;
using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueSharpSadConsoleSamples.Core;
using RogueSharp.Random;

namespace RogueSharpSadConsoleSamples.Systems
{
   public class MapGenerator
   {
      private readonly int _width;
      private readonly int _height;
      private readonly int _maxRooms;
      private readonly int _roomMaxSize;
      private readonly int _roomMinSize;
      private readonly int _level;
      private readonly DungeonMap _map;

      public static IRandom? Random { get; private set; }

      public MapGenerator( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level )
      {
         _width = width;
         _height = height;
         _maxRooms = maxRooms;
         _roomMaxSize = roomMaxSize;
         _roomMinSize = roomMinSize;
         _level = level;
         //create seed
        int seed = (int) DateTime.UtcNow.Ticks;
        Random = new DotNetRandom( seed );
         _map = new DungeonMap();
      }

      public DungeonMap CreateMap()
      {
         _map.Initialize( _width, _height );

         for ( int r = 0; r < _maxRooms; r++ )
         {
            int roomWidth = Random!.Next( _roomMinSize, _roomMaxSize );
            int roomHeight = Random.Next( _roomMinSize, _roomMaxSize );
            int roomXPosition = Random.Next( 0, _width - roomWidth - 1 );
            int roomYPosition = Random.Next( 0, _height - roomHeight - 1 );

            var newRoom = new Rectangle( roomXPosition, roomYPosition, roomWidth, roomHeight );
            bool newRoomIntersects = _map.Rooms.Any( room => newRoom.Intersects( room ) );
            if ( !newRoomIntersects )
            {
               _map.Rooms.Add( newRoom );
            }
         }

         foreach ( Rectangle room in _map.Rooms )
         {
            CreateMap( room );
         }

         for ( int r = 0; r < _map.Rooms.Count; r++ )
         {
            if ( r == 0 )
            {
               continue;
            }

            int previousRoomCenterX = _map.Rooms[r - 1].Center.X;
            int previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
            int currentRoomCenterX = _map.Rooms[r].Center.X;
            int currentRoomCenterY = _map.Rooms[r].Center.Y;

            if ( Random!.Next( 0, 2 ) == 0 )
            {
               CreateHorizontalTunnel( previousRoomCenterX, currentRoomCenterX, previousRoomCenterY );
               CreateVerticalTunnel( previousRoomCenterY, currentRoomCenterY, currentRoomCenterX );
            }
            else
            {
               CreateVerticalTunnel( previousRoomCenterY, currentRoomCenterY, previousRoomCenterX );
               CreateHorizontalTunnel( previousRoomCenterX, currentRoomCenterX, currentRoomCenterY );
            }
         }

        
        
         foreach ( Rectangle room in _map.Rooms )
         {
            CreateDoors( room );
         }

         return _map;
      }

      private void CreateMap( Rectangle room )
      {
         for ( int x = room.Left + 1; x < room.Right; x++ )
         {
            for ( int y = room.Top + 1; y < room.Bottom; y++ )
            {
               _map.SetCellProperties( x, y, true, true );
            }
         }
      }

      private void CreateHorizontalTunnel( int xStart, int xEnd, int yPosition )
      {
         for ( int x = Math.Min( xStart, xEnd ); x <= Math.Max( xStart, xEnd ); x++ )
         {
            _map.SetCellProperties( x, yPosition, true, true );
         }
      }

      private void CreateVerticalTunnel( int yStart, int yEnd, int xPosition )
      {
         for ( int y = Math.Min( yStart, yEnd ); y <= Math.Max( yStart, yEnd ); y++ )
         {
            _map.SetCellProperties( xPosition, y, true, true );
         }
      }
      
      private void CreateDoors( Rectangle room )
      {
         int xMin = room.Left;
         int xMax = room.Right;
         int yMin = room.Top;
         int yMax = room.Bottom;
     }
   }
}