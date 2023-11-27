using Godot;
using System;
using System.Collections.Generic;

public partial class ClaimedCellCollection : RefCounted
{
	private Dictionary<Vector2I, StringName> ClaimedCells = new Dictionary<Vector2I, StringName>();
	
	public ClaimedCellCollection() { }
	
	public void ClaimCell(Vector2I cell, StringName id)
	{
		if(id == null) throw new ArgumentException();
		ClaimedCells[cell] = id;
	}
	public void ClaimCells(IEnumerable<Vector2I> cells, StringName id)
	{
		if(id == null) throw new ArgumentException();
		foreach(Vector2I cell in cells)
		{
			ClaimedCells[cell] = id;
		}
	}
	public void ClaimCells(IEnumerable<Vector2I> cells, IEnumerable<StringName> ids)
	{
		ClaimCells(new List<Vector2I>(cells), new List<StringName>(ids));
	}
	public void ClaimCells(IList<Vector2I> cells, IList<StringName> ids)
	{
		if(cells.Count != ids.Count) throw new ArgumentException();
		if(ids.Contains(null)) throw new ArgumentException();
		for(int i = 0; i < cells.Count; i++)
		{
			ClaimedCells[cells[i]] = ids[i];
		}
	}
	
	public bool TryGetClaimID(Vector2I cell, out StringName claimID)
	{
		return ClaimedCells.TryGetValue(cell, out claimID);
	}
	// Get claim ID for cell if claimed, else return null
	public StringName GetClaimID(Vector2I cell)
	{
		if(ClaimedCells.TryGetValue(cell, out StringName claimID))
		{
			return claimID;
		}
		return null;
	}
	// Get a list of claim IDs parallel to given cells
	// Elements in the returned array parallel to unclaimed cells are null
	public List<StringName> GetCellClaimIDs(IEnumerable<Vector2I> cells)
	{
		List<StringName> claimIDs = new List<StringName>();
		foreach(Vector2I cell in cells)
		{
			claimIDs.Add(GetClaimID(cell));
		}
		return claimIDs;
	}
	// Get a set of all claim IDs found from the cells
	public HashSet<StringName> GetClaimIDSet(IEnumerable<Vector2I> cells)
	{
		HashSet<StringName> claimIDs = new HashSet<StringName>();
		foreach(Vector2I cell in cells)
		{
			if(ClaimedCells.TryGetValue(cell, out StringName claimID))
			{
				claimIDs.Add(claimID);
			}
		}
		return claimIDs;
	}
	
	// Check if any cell in the collection has the specified claim ID
	public bool ContainsClaimID(IEnumerable<Vector2I> cells, StringName claimID)
	{
		return ContainsClaimIDs(cells, new StringName[] { claimID });
	}
	// Check if nay cell in the collection has any of the specified claim IDs
	public bool ContainsClaimIDs(IEnumerable<Vector2I> cells, IEnumerable<StringName> claimIDs)
	{
		HashSet<StringName> cellIDs = GetClaimIDSet(cells);
		foreach(StringName claimID in claimIDs)
		{
			if(cellIDs.Contains(claimID))
			{
				return true;
			}
		}
		return false;
	}
	
	public bool CellIsClaimed(Vector2I cell)
	{
		return ClaimedCells.ContainsKey(cell);
	}
	public bool ContainsClaimedCells(IEnumerable<Vector2I> cells)
	{
		foreach(Vector2I cell in cells)
		{
			if(ClaimedCells.ContainsKey(cell))
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
			if(ClaimedCells.ContainsKey(cell))
			{
				claimed.Add(cell);
			}
		}
		return claimed;
	}
	
	
	public void ClaimCells(Vector2I drawPoint, TileMapPattern pattern, StringName id)
	{
		ClaimCells(GetPatternCells(drawPoint, pattern), id);
	}
	public bool ContainsClaimedCells(Vector2I drawPoint, TileMapPattern pattern)
	{
		return ContainsClaimedCells(GetPatternCells(drawPoint, pattern));
	}
	public List<Vector2I> GetClaimedCells(Vector2I drawPoint, TileMapPattern pattern)
	{
		return GetClaimedCells(GetPatternCells(drawPoint, pattern));
	}
	public HashSet<StringName> GetClaimIDSet(Vector2I drawPoint, TileMapPattern pattern)
	{
		return GetClaimIDSet(GetPatternCells(drawPoint, pattern));
	}
	public bool ContainsClaimID(Vector2I drawPoint, TileMapPattern pattern, StringName claimID)
	{
		return ContainsClaimID(GetPatternCells(drawPoint, pattern), claimID);
	}
	public bool ContainsClaimIDs(Vector2I drawPoint, TileMapPattern pattern, IEnumerable<StringName> claimIDs)
	{
		return ContainsClaimIDs(GetPatternCells(drawPoint, pattern), claimIDs);
	}
	
	private static List<Vector2I> GetPatternCells(Vector2I drawPoint, TileMapPattern pattern)
	{
		List<Vector2I> cells = new List<Vector2I>();
		foreach(Vector2I cell in pattern.GetUsedCells())
		{
			cells.Add(cell + drawPoint);
		}
		return cells;
	}
	
}
