using Godot;
using System;
using System.Collections.Generic;

public struct PathPointTemplate
{
	public HashSet<Vector2I> Floors;
	public HashSet<Vector2I> Walls;
	
	public PathPointTemplate(IEnumerable<Vector2I> floors, IEnumerable<Vector2I> walls)
	{
		Floors = new HashSet<Vector2I>(floors);
		Walls = new HashSet<Vector2I>(walls);
	}
	
	public PathPointTemplate(int thickness, bool hasWalls)
	{
		Vector2 center = thickness % 2 == 1 ? Vector2.Zero : new Vector2(-0.5f, -0.5f);
		
		Floors = new HashSet<Vector2I>();
		// Add all cells within thickness
		for(int x = GetMin(thickness); x < GetMax(thickness); x++)
		{
			for(int y = GetMin(thickness); y < GetMax(thickness); y++)
			{
				Vector2I cell = new Vector2I(x, y);
				float length = ((Vector2)cell - center).Length();
				if(length <= thickness / 2f)
				{
					Floors.Add(cell);
				}
			}
		}
		
		Walls = new HashSet<Vector2I>();
		if(!hasWalls) return;
		// Add wall cells around the floors
		// Wall outline is 2 cells wide and 3 cells tall
		for(int x = GetMin(thickness + 2) - 1; x < GetMax(thickness + 2) + 1; x++)
		{
			for(int y = GetMin(thickness + 2) - 2; y < GetMax(thickness + 2) + 2; y++)
			{
				Vector2I cell = new Vector2I(x, y);
				if(Floors.Contains(cell)) continue;
				bool hasFloorNeighbor = false;
				for(int i = -2; i <= 2; i++)
				{
					for(int j = -3; j <= 3; j++)
					{
						if(Floors.Contains(cell + new Vector2I(i, j)))
						{
							hasFloorNeighbor = true;
							break;
						}
					}
					if(hasFloorNeighbor) break;
				}
				if(hasFloorNeighbor)
				{
					Walls.Add(cell);
				}
			}
		}
	}
	
	private static int GetMin(int thickness) => -thickness / 2;
	private static int GetMax(int thickness) => (thickness + 1) / 2;
}
