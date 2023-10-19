using Godot;
using System;

namespace ProgJam2023;

public enum Direction
{
   None,
   Up,
   Down,
   Left,
   Right
}

public static class Utils
{
   public static Vector2I DirectionToVector(Direction direction)
   {
      switch (direction)
      {
         case Direction.Up:
            return new Vector2I(0, -1);
         case Direction.Down:
            return new Vector2I(0, 1);
         case Direction.Left:
            return new Vector2I(-1, 0);
         case Direction.Right:
            return new Vector2I(1, 0);
      }

      return new Vector2I(0, 0);
   }

   public static Direction VectorToDirection(Vector2 vector)
   {
      if (vector.X != 0 && vector.Y != 0)
      {
         return Direction.None;
      }

      if (vector.X > 0)
      {
         return Direction.Right;
      }

      if (vector.X < 0)
      {
         return Direction.Left;
      }

      if (vector.Y > 0)
      {
         return Direction.Down;
      }

      if (vector.Y < 0)
      {
         return Direction.Up;
      }

      return Direction.None;
   }

   public static Direction VectorToDirection(Vector2I vector)
   {
      if (vector.X != 0 && vector.Y != 0)
      {
         return Direction.None;
      }

      if (vector.X > 0)
      {
         return Direction.Right;
      }

      if (vector.X < 0)
      {
         return Direction.Left;
      }

      if (vector.Y > 0)
      {
         return Direction.Down;
      }

      if (vector.Y < 0)
      {
         return Direction.Up;
      }

      return Direction.None;
   }
}