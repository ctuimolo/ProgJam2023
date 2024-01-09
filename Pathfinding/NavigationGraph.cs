using Godot;
using System;

using System.Collections.Generic;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public class NavigationGraph
{
	private AStar2D AStar;
	
	private int LastID;
	
	private Dictionary<Cell, int> CellToID;
	private Dictionary<int, Cell> IDToCell;
	public IEnumerable<Cell> Cells => CellToID.Keys;
	
	public NavigationGraph()
	{
		AStar = new AStar2D();
		
		LastID = 0;
		CellToID = new Dictionary<Cell, int>();
		IDToCell = new Dictionary<int, Cell>();
	}
	
	public Path GetPath(Cell start, Cell end)
	{
		int startID = CellToID[start];
		int endID = CellToID[end];
		long[] idPath = AStar.GetIdPath(startID, endID);
		Path path = new Path();
		foreach(int id in idPath)
		{
			path.Cells.Add(IDToCell[id]);
		}
		return path;
	}
	
	public bool HasCell(Cell cell)
	{
		return CellToID.ContainsKey(cell);
	}
	
	public void AddCell(Cell cell, float weight)
	{
		int id = LastID++;
		AStar.AddPoint(id, cell.Position(), weight);
		CellToID[cell] = id;
		IDToCell[id] = cell;
	}
	
	public void RemoveCell(Cell cell)
	{
		if(!CellToID.TryGetValue(cell, out int id))
		{
			throw new ArgumentException();
		}
		AStar.RemovePoint(id);
		CellToID.Remove(cell);
		IDToCell.Remove(id);
	}
	
	public void SetWeight(Cell cell, float weight)
	{
		if(!CellToID.TryGetValue(cell, out int id))
		{
			throw new ArgumentException();
		}
		AStar.SetPointWeightScale(id, weight);
	}
	
	public void ConnectCells(Cell a, Cell b, bool bidirectional)
	{
		if(!CellToID.TryGetValue(a, out int aID) || !CellToID.TryGetValue(b, out int bID))
		{
			throw new ArgumentException();
		}
		AStar.ConnectPoints(aID, bID, bidirectional);
	}
	public void DisconnectCells(Cell a, Cell b, bool bidirectional)
	{
		if(!CellToID.TryGetValue(a, out int aID) || !CellToID.TryGetValue(b, out int bID))
		{
			throw new ArgumentException();
		}
		AStar.DisconnectPoints(aID, bID);
	}
	public bool CellsAreConnected(Cell a, Cell b, bool bidirectional)
	{
		if(!CellToID.TryGetValue(a, out int aID) || !CellToID.TryGetValue(b, out int bID))
		{
			throw new ArgumentException();
		}
		return AStar.ArePointsConnected(aID, bID, bidirectional);
	}
	
	public void Clear()
	{
		AStar.Clear();
	}
	
}
