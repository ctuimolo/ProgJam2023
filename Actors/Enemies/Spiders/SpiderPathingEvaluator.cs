using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Pathfinding;
using ProgJam2023.Rooms;

namespace ProgJam2023.Actors.Enemies.Spiders;

public class SpiderPathingEvaluator : PathingEvaluator
{	
	public SpiderPathingEvaluator(Room room) : base(room) { }
	
	public override float GetCurrentWeight(Cell cell)
	{
		if(!IsTraversable(cell))
		{
			return float.PositiveInfinity;
		}
		if(cell.HasCollisions(false))
		{
			return float.PositiveInfinity;
		}
		return 1f;
	}
	
	public override bool IsTraversable(Cell cell)
	{
		foreach(GridActor actor in cell.Actors)
		{
			if(actor is Door)
			{
				return false;
			}
		}
		return true;
	}
	
	public override IList<Cell> GetNeighbors(Cell cell)
	{
		List<Cell> neighbors = new List<Cell>();
		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				if(x == 0 && y == 0)
				{
					continue;
				}
				
				Vector2I position = cell.Position() + new Vector2I(x, y);
				if(Room.CellMap.TryGetValue(position, out Cell neighbor))
				{
					if(IsTraversable(neighbor))
					{
						neighbors.Add(neighbor);
					}
				}
			}
		}
		return neighbors;
	}
	
	public override bool CanMove(Cell from, Cell to)
	{
		if(from.Position() == to.Position())
		{
			return false;
		}
		if(float.IsPositiveInfinity(GetCurrentWeight(to)))
		{
			return false;
		}
		Vector2I difference = to.Position() - from.Position();
		return Math.Abs(difference.X) < 1 && Math.Abs(difference.Y) < 1;
	}
}
