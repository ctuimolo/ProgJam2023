using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public class Path
{
	public List<Cell> Cells;
	public int Count => Cells.Count;
	public void Clear() => Cells.Clear();
	public Cell this[int index] => Cells[index];
	
	public Path()
	{
		Cells = new List<Cell>();
	}
	
	public Path(IEnumerable<Cell> cells)
	{
		Cells = new List<Cell>(cells);
	}
}
