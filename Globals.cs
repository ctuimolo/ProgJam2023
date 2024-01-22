using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors;
using ProgJam2023.World;

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
   public static RandomNumberGenerator RNG = new RandomNumberGenerator();

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

	public static GridDirection GetRandomMoveDirection(GridActor actor)
	{
		List<GridDirection> directions = new List<GridDirection>();
		if(WorldManager.TestTraversable(actor, GridDirection.Up))
		{
			directions.Add(GridDirection.Up);
		}
		if(WorldManager.TestTraversable(actor, GridDirection.Down))
		{
			directions.Add(GridDirection.Down);
		}
		if(WorldManager.TestTraversable(actor, GridDirection.Left))
		{
			directions.Add(GridDirection.Left);
		}
		if(WorldManager.TestTraversable(actor, GridDirection.Right))
		{
			directions.Add(GridDirection.Right);
		}
		
		if(directions.Count == 0)
		{
			return GridDirection.None;
		}
		return directions[RandomRange(directions.Count)];
	}
	public static int RandomRange(int min, int max)
	{
		if(min == max) return min;
		return (int)((GD.Randi() % (max - min)) + min);
	}
	public static int RandomRange(int max) => RandomRange(0, max);
}
