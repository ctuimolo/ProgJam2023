using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;
using ProgJam2023.Actors;

namespace ProgJam2023.Pathfinding;

public class BasicPathingEvaluator : PathingEvaluator
{
	public BasicPathingEvaluator(Room room) : base(room) { }
	
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
		AddNeighborIfTraversable(cell.Position() + new Vector2I(0, 1), neighbors);
		AddNeighborIfTraversable(cell.Position() + new Vector2I(1, 0), neighbors);
		AddNeighborIfTraversable(cell.Position() + new Vector2I(0, -1), neighbors);
		AddNeighborIfTraversable(cell.Position() + new Vector2I(-1, 0), neighbors);
		return neighbors;
	}
	private void AddNeighborIfTraversable(Vector2I position, List<Cell> neighbors)
	{
		if(Room.CellMap.TryGetValue(position, out Cell neighbor))
		{
			if(IsTraversable(neighbor))
			{
				neighbors.Add(neighbor);
			}
		}
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
		bool xIsOne = Math.Abs(difference.X) == 1;
		bool yIsOne = Math.Abs(difference.Y) == 1;
		return xIsOne != yIsOne;
	}
}
