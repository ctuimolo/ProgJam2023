using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public class NavigationGraphBuilder
{
	
	public NavigationGraph NavigationGraph;
	public PathingEvaluator PathingEvaluator;
	public Room Room;
	
	public NavigationGraphBuilder(NavigationGraph navigationGraph, PathingEvaluator pathingEvaluator, Room room)
	{
		NavigationGraph = navigationGraph;
		PathingEvaluator = pathingEvaluator;
		Room = room;
	}
	
	public void BuildGraph()
	{
		ClearGraph();
		AddTraversableCells();
		ConnectCells();
	}
	
	private void AddTraversableCells()
	{
		foreach(Cell cell in Room.CellMap.Values)
		{
			if(PathingEvaluator.IsTraversable(cell))
			{
				NavigationGraph.AddCell(cell, PathingEvaluator.GetCurrentWeight(cell));
			}
		}
	}
	
	private void ConnectCells()
	{
		foreach(Cell cell in NavigationGraph.Cells)
		{
			IList<Cell> neighbors = PathingEvaluator.GetNeighbors(cell);
			foreach(Cell neighbor in neighbors)
			{
				NavigationGraph.ConnectCells(cell, neighbor, false);
			}
		}
	}
	
	public void UpdateWeights()
	{
		foreach(Cell cell in NavigationGraph.Cells)
		{
			NavigationGraph.SetWeight(cell, PathingEvaluator.GetCurrentWeight(cell));
		}
	}
	
	public void ClearGraph()
	{
		NavigationGraph.Clear();
	}
}
