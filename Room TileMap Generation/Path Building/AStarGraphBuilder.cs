using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

public partial class AStarGraphBuilder : RefCounted
{
	public TileMap TileMap;
	public HashSet<Vector2I> Cells;
	
	public AStar2D AStar;
	
	private Dictionary<Vector2I, int> CellToID = new Dictionary<Vector2I, int>();
	
	public AStarGraphBuilder(TileMap tileMap, HashSet<Vector2I> cells, AStar2D aStar)
	{
		TileMap = tileMap;
		Cells = cells;
		AStar = aStar;
	}
	
	public int GetID(Vector2I cell)
	{
		return CellToID[cell];
	}
	
	public void BuildGraph(ICellEvaluator evaluator)
	{
		AStar.Clear();
		
		int count = 0;
		CellToID.Clear();
		
		HashSet<Vector2I> navigable = new HashSet<Vector2I>();
		
		foreach(Vector2I cell in Cells)
		{
			float weight = evaluator.GetWeight(cell);
			int id = count++;
			AStar.AddPoint(id, cell, weight);
			CellToID[cell] = id;
			if(!float.IsPositiveInfinity(weight))
			{
				navigable.Add(cell);
			}
		}
		
		foreach(Vector2I cell in navigable)
		{
			int id = CellToID[cell];
			
			IList<Vector2I> neighbors = evaluator.GetNeighbors(cell);
			foreach(Vector2I neighbor in neighbors)
			{
				if(!navigable.Contains(neighbor))
				{
					continue;
				}
				AStar.ConnectPoints(id, CellToID[neighbor], false);
			}
		}
	}
}

public interface ICellEvaluator
{
	float GetWeight(Vector2I cell);
	IList<Vector2I> GetNeighbors(Vector2I cell);
}
