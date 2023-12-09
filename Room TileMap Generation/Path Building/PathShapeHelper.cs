using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

public static class PathShapeHelper
{
	// Circle center point is offset if diameter is even
	public static Vector2 GetCircleCenter(int diameter)
	{
		return diameter % 2 == 1 ? Vector2.Zero : new Vector2(-0.5f, -0.5f);
	}
	
	public static int GetCircleMin(int thickness) => -thickness / 2;
	public static int GetCircleMax(int thickness) => (thickness + 1) / 2;
	
	public static HashSet<Vector2I> GetCircle(int diameter) => GetCircle(diameter, Vector2I.Zero);
	public static HashSet<Vector2I> GetCircle(int diameter, Vector2I offset)
	{
		Vector2 center = GetCircleCenter(diameter);
		
		HashSet<Vector2I> cells = new HashSet<Vector2I>();
		for(int x = GetCircleMin(diameter); x < GetCircleMax(diameter); x++)
		{
			for(int y = GetCircleMin(diameter); y < GetCircleMax(diameter); y++)
			{
				Vector2I cell = new Vector2I(x, y);
				
				float length = ((Vector2)cell - center).Length();
				if(length <= diameter / 2f)
				{
					cells.Add(cell + offset);
				}
			}
		}
		return cells;
	}
	
	public static HashSet<Vector2I> GetOutline(int xOffset, int yOffset, HashSet<Vector2I> shape, Vector2I shapeMin, Vector2I shapeMax)
	{
		HashSet<Vector2I> cells = new HashSet<Vector2I>();
		for(int x = shapeMin.X - xOffset; x < shapeMax.X + xOffset; x++)
		{
			for(int y = shapeMin.Y - yOffset; y < shapeMax.Y + yOffset; y++)
			{
				Vector2I cell = new Vector2I(x, y);
				if(shape.Contains(cell)) continue;
				bool hasShapeNeighbor = false;
				for(int i = -xOffset; i <= xOffset; i++)
				{
					for(int j = -yOffset; j <= yOffset; j++)
					{
						if(shape.Contains(cell + new Vector2I(i, j)))
						{
							hasShapeNeighbor = true;
							break;
						}
					}
					if(hasShapeNeighbor) break;
				}
				if(hasShapeNeighbor)
				{
					cells.Add(cell);
				}
			}
		}
		return cells;
	}
	public static HashSet<Vector2I> GetOutline(int xOffset, int yOffset, HashSet<Vector2I> shape)
	{
		Rect2I bounds = FindBounds(shape);
		return GetOutline(xOffset, yOffset, shape, bounds.Position, bounds.End);
	}
	
	// Get Vector2I with minimum X and Y coordinates found in the shape
	public static Vector2I FindMin(HashSet<Vector2I> shape) => FindBounds(shape).Position;
	// Get Vector2I with maximum X and Y coordinates found in the shape
	public static Vector2I FindMax(HashSet<Vector2I> shape) => FindBounds(shape).End;
	
	public static Rect2I FindBounds(HashSet<Vector2I> shape)
	{
		int minX = int.MaxValue;
		int minY = int.MaxValue;
		int maxX = int.MinValue;
		int maxY = int.MinValue;
		foreach(Vector2I cell in shape)
		{
			minX = Math.Min(minX, cell.X);
			minY = Math.Min(minY, cell.Y);
			maxX = Math.Max(maxX, cell.X);
			maxY = Math.Max(maxY, cell.Y);
		}
		Vector2I min = new Vector2I(minX, minY);
		Vector2I max = new Vector2I(maxX, maxY);
		return new Rect2I(min, max - min);
	}
	
	public static Vector2 FindCenter(HashSet<Vector2I> shape)
	{
		Rect2I bounds = FindBounds(shape);
		return new Vector2(
			(bounds.Position.X + bounds.End.X) / 2f,
			(bounds.Position.Y + bounds.End.Y) / 2f
		);
	}
	
}
