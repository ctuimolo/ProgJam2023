using Godot;
using System;
using System.Collections.Generic;

public partial class SimplePathBuilder : Node
{
	private RandomNumberGenerator RNG;
	
	public SimplePathBuilder(RandomNumberGenerator rng)
	{
		RNG = rng;
	}
	
	public List<Vector2I> GetPathBetween(Vector2I a, Vector2I b)
	{
		Vector2I difference = b - a;
		Vector2I stepX = new Vector2I(difference.X > 0 ? 1 : -1, 0);
		Vector2I stepY = new Vector2I(0, difference.Y > 0 ? 1 : -1);
		Vector2I coords = a;
		List<Vector2I> result = new List<Vector2I>();
		while(coords != a)
		{
			result.Add(coords);
			if(coords.X == b.X)
			{
				coords += stepY;
			}
			else if(coords.Y == b.Y)
			{
				coords += stepX;
			}
			else
			{
				coords += RNG.RandiRange(0, 1) == 0 ? stepY : stepX;
			}
		}
		result.Add(b);
		return result;
	}
	
	public List<Vector2I> GetPathBetweenBidirectional(Vector2I a, Vector2I b)
	{
		Vector2I difference = b - a;
		Vector2I stepX = new Vector2I(difference.X > 0 ? 1 : -1, 0);
		Vector2I stepY = new Vector2I(0, difference.Y > 0 ? 1 : -1);
		Vector2I coordsA = a;
		Vector2I coordsB = b;
		List<Vector2I> pathA = new List<Vector2I>() { a };
		List<Vector2I> pathB = new List<Vector2I>() { b };
		while(coordsA != coordsB)
		{
			// Step A
			if(coordsA.X == coordsB.X)
			{
				coordsA += stepY;
			}
			else if(coordsA.Y == coordsB.Y)
			{
				coordsA += stepX;
			}
			else
			{
				coordsA += RNG.RandiRange(0, 1) == 0 ? stepY : stepX;
			}
			
			if(coordsA == coordsB)
			{
				break;
			}
			pathA.Add(coordsA);
			
			// Step B
			if(coordsA.X == coordsB.X)
			{
				coordsB -= stepY;
			}
			else if(coordsA.Y == coordsB.Y)
			{
				coordsB -= stepX;
			}
			else
			{
				coordsB -= RNG.RandiRange(0, 1) == 0 ? stepY : stepX;
			}
			
			if(coordsA == coordsB)
			{
				break;
			}
			pathB.Add(coordsB);
		}
		
		List<Vector2I> result = pathA;
		pathB.Reverse();
		result.AddRange(pathB);
		return result;
	}
	
}
