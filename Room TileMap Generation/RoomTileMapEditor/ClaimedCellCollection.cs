using Godot;
using System;
using System.Collections.Generic;

public partial class ClaimedCellCollection : RefCounted
{
	private HashSet<Vector2I> ClaimedCells = new HashSet<Vector2I>();
	
	public ClaimedCellCollection() { }
	
	public void ClaimCell(Vector2I cell)
	{
		if(ClaimedCells.Contains(cell))
		{
			throw new ArgumentException($"Cell {cell} is already claimed");
		}
		ClaimedCells.Add(cell);
	}
	public void ClaimCells(IEnumerable<Vector2I> cells)
	{
		foreach(Vector2I cell in cells)
		{
			if(ClaimedCells.Contains(cell))
			{
				throw new ArgumentException($"Cell {cell} is already claimed");
			}
		}
		foreach(Vector2I cell in cells)
		{
			ClaimedCells.Add(cell);
		}
	}
	public void ClaimPatternArea(Vector2I drawPosition, TileMapPattern pattern)
	{
		List<Vector2I> cells = new List<Vector2I>();
		foreach(Vector2I cell in pattern.GetUsedCells())
		{
			cells.Add(cell + drawPosition);
		}
		ClaimCells(cells);
	}
	
	public bool CellIsClaimed(Vector2I cell)
	{
		return ClaimedCells.Contains(cell);
	}
	public bool ContainsClaimedCells(IEnumerable<Vector2I> cells)
	{
		foreach(Vector2I cell in cells)
		{
			if(ClaimedCells.Contains(cell))
			{
				return true;
			}
		}
		return false;
	}
	public List<Vector2I> GetClaimedCells(IEnumerable<Vector2I> cells)
	{
		List<Vector2I> claimed = new List<Vector2I>();
		foreach(Vector2I cell in cells)
		{
			if(ClaimedCells.Contains(cell))
			{
				claimed.Add(cell);
			}
		}
		return claimed;
	}
	
	public bool PatternAreaContainsClaimedCells(Vector2I drawPoint, TileMapPattern pattern)
	{
		foreach(Vector2I cell in pattern.GetUsedCells())
		{
			if(ClaimedCells.Contains(cell + drawPoint))
			{
				return true;
			}
		}
		return false;
	}
	public List<Vector2I> GetClaimedCellsInPatternArea(Vector2I drawPoint, TileMapPattern pattern)
	{
		List<Vector2I> cells = new List<Vector2I>();
		foreach(Vector2I cell in pattern.GetUsedCells())
		{
			if(ClaimedCells.Contains(cell + drawPoint))
			{
				cells.Add(cell + drawPoint);
			}
		}
		return cells;
	}
	
}
