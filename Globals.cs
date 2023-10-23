using Godot;
using Godot.Collections;

namespace ProgJam2023;

public enum GridDirection
{
   None,
   Up,
   Down,
   Left,
   Right
}

public static class Utils
{
   public static Vector2I DirectionToVector(GridDirection direction)
   {
      switch (direction)
      {
         case GridDirection.Up:
            return new Vector2I(0, -1);
         case GridDirection.Down:
            return new Vector2I(0, 1);
         case GridDirection.Left:
            return new Vector2I(-1, 0);
         case GridDirection.Right:
            return new Vector2I(1, 0);
      }

      return new Vector2I(0, 0);
   }

   public static GridDirection VectorToDirection(Vector2 vector)
   {
      if (vector.X != 0 && vector.Y != 0)
      {
         return GridDirection.None;
      }

      if (vector.X > 0)
      {
         return GridDirection.Right;
      }

      if (vector.X < 0)
      {
         return GridDirection.Left;
      }

      if (vector.Y > 0)
      {
         return GridDirection.Down;
      }

      if (vector.Y < 0)
      {
         return GridDirection.Up;
      }

      return GridDirection.None;
   }

   public static GridDirection VectorToDirection(Vector2I vector)
   {
      Vector2 tmp = new Vector2(vector.X, vector.Y);
      return VectorToDirection(tmp);
   }
}