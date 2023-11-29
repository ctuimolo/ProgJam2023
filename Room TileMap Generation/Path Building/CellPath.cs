using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

public class CellPath
{
	public List<Vector2I> Points = new List<Vector2I>();
	
	public HashSet<Vector2I> Floors = new HashSet<Vector2I>();
	public HashSet<Vector2I> Walls = new HashSet<Vector2I>();
	
	public CellPath(List<Vector2I> points, HashSet<Vector2I> floors = null, HashSet<Vector2I> walls = null)
	{
		Points = new List<Vector2I>(points);
		if(floors != null)
		{
			Floors = new HashSet<Vector2I>(floors);
		}
		if(walls != null)
		{
			Walls = new HashSet<Vector2I>(walls);
		}
	}
	public CellPath(CellPath other)
	{
		Points = new List<Vector2I>(other.Points);
		Floors = new HashSet<Vector2I>(other.Floors);
		Walls = new HashSet<Vector2I>(other.Walls);
	}
	public CellPath(IEnumerable<Vector2I> points, PathPointTemplate template)
	{
		Points = new List<Vector2I>(points);
		foreach(Vector2I point in Points)
		{
			foreach(Vector2I offset in template.Floors)
			{
				Floors.Add(point + offset);
			}
		}
		foreach(Vector2I point in Points)
		{
			foreach(Vector2I offset in template.Walls)
			{
				Vector2I cell = point + offset;
				if(!Floors.Contains(cell))
				{
					Walls.Add(cell);
				}
			}
		}
	}
}
