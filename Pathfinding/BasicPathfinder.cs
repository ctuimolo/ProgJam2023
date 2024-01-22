using Godot;
using System;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public partial class BasicPathfinder : Pathfinder
{
	public Room Room;
	public PathingEvaluator PathingEvaluator;
	public NavigationGraph NavigationGraph;
	public NavigationGraphBuilder NavigationGraphBuilder;
	
	public BasicPathfinder(Room room, PathingEvaluator pathingEvaluator, NavigationGraph navigationGraph, NavigationGraphBuilder navigationGraphBuilder)
	{
		Room = room;
		PathingEvaluator = pathingEvaluator;
		NavigationGraph = navigationGraph;
		NavigationGraphBuilder = navigationGraphBuilder;
	}
	
	public override Path GetPath(Cell start, Cell end)
	{
		return NavigationGraph.GetPath(start, end);
	}
	
	public override bool PathIsValid(Path path, int steps = -1)
	{
		if(steps < 0) steps = path.Count;
		for(int i = 0; i < path.Count && i < steps; i++)
		{
			Cell cell = path[i];
			
			if(!PathingEvaluator.IsTraversable(cell))
			{
				return false;
			}
			
			if(float.IsPositiveInfinity(PathingEvaluator.GetCurrentWeight(cell)))
			{
				return false;
			}
			
			if(i > 0 && !PathingEvaluator.CanMove(path[i - 1], cell))
			{
				return false;
			}
		}
		return true;
	}
}
