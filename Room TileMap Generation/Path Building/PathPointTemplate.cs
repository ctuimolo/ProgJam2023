using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

using static PathShapeHelper;

public struct PathPointTemplate
{
	public HashSet<Vector2I> Floors;
	public HashSet<Vector2I> Walls;
	
	public PathPointTemplate(IEnumerable<Vector2I> floors, IEnumerable<Vector2I> walls)
	{
		Floors = new HashSet<Vector2I>(floors);
		Walls = new HashSet<Vector2I>(walls);
	}
	
	public PathPointTemplate(int diameter, bool hasWalls)
	{
		Floors = GetCircle(diameter);
		
		if(!hasWalls)
		{
			Walls = new HashSet<Vector2I>();
		}
		else
		{
			Vector2I min = new Vector2I(GetCircleMin(diameter), GetCircleMin(diameter));
			Vector2I max = new Vector2I(GetCircleMax(diameter), GetCircleMax(diameter));
			Walls = GetOutline(2, 3, Floors, min, max);
		}
	}
}
