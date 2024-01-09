using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public abstract class PathingEvaluator
{
	public Room Room;
	
	public PathingEvaluator(Room room)
	{
		Room = room;
	}
	
	// Current weight of the cell
	public abstract float GetCurrentWeight(Cell cell);
	
	// False only if cell is never traversable
	public abstract bool IsTraversable(Cell cell);
	
	// Get all cells accessible from the given cell
	public abstract IList<Cell> GetNeighbors(Cell cell);
	
	public abstract bool CanMove(Cell from, Cell to);
}
