using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Bats;

public partial class Bat : Enemy
{
	
	[Export]
	public int ChaseDistance = 3;
	
	[Export]
	public int SpiralLengthMin = 3;
	[Export]
	public int SpiralLengthMax = 6;
	
	private bool NavigationInitialized = false;
	
	private BasicPathfinder Pathfinder;
	
	private Path CurrentPath;
	private int PathIndex;
	
	public override void _Ready()
	{
		base._Ready();
		
		if(!NavigationInitialized && CurrentRoom != null)
		{
			InitializeNavigation();
		}
	}
	
	private void InitializeNavigation()
	{
		if(NavigationInitialized)
		{
			throw new InvalidOperationException();
		}
		NavigationInitialized = true;
		
		NavigationGraph graph = new NavigationGraph();
		BasicPathingEvaluator pathingEvaluator = new BasicPathingEvaluator(CurrentRoom);
		NavigationGraphBuilder graphBuilder = new NavigationGraphBuilder(graph, pathingEvaluator, CurrentRoom);
		graphBuilder.BuildGraph();
		Pathfinder = new BasicPathfinder(CurrentRoom, pathingEvaluator, graph, graphBuilder);
	}
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if(!NavigationInitialized && CurrentRoom != null)
		{
			InitializeNavigation();
		}
	}
	
	public override void TakeTurn()
	{
		if(!NavigationInitialized && CurrentRoom != null)
		{
			InitializeNavigation();
		}
		
		if(!_needsToTakeTurn)
		{
			return;
		}
		
		if(!NavigationInitialized)
		{
			throw new InvalidOperationException("CurrentRoom not set!");
		}
		
		// Navigate to player if nearby
		Vector2I distanceToPlayer = WorldManager.CurrentPlayer.CurrentCell.Position() - CurrentCell.Position();
		if(Math.Abs(distanceToPlayer.X) + Math.Abs(distanceToPlayer.Y) <= ChaseDistance)
		{
			//GD.Print("Chasing player");
			Pathfinder.NavigationGraphBuilder.UpdateWeights();
			CurrentPath = Pathfinder.GetPath(CurrentCell, WorldManager.CurrentPlayer.CurrentCell);
			PathIndex = 1;
		}
		
		// Make spiral path if no CurrentPath or next Cell is blocked
		if(CurrentPath == null || PathIndex >= CurrentPath.Count || !WorldManager.TestTraversable(this, CurrentPath[PathIndex]))
		{
			CurrentPath = GetSpiralPath();
			PathIndex = 1;
		}
		
		// Move on path
		if(CurrentPath != null && PathIndex < CurrentPath.Count)
		{
			Vector2I vector = CurrentPath[PathIndex].Position() - CurrentCell.Position();
			GridDirection direction = Utils.VectorToDirection(vector);
			if(WorldManager.TryMoveActor(this, direction))
			{
				//GD.Print("Following path");
				PathIndex++;
			}
		}
		else
		{
			//GD.Print("Can't follow path");
			GridDirection randomDirection = Utils.GetRandomMoveDirection(this);
			if(randomDirection != GridDirection.None)
			{
				WorldManager.TryMoveActor(this, randomDirection);
			}
		}
		
		_needsToTakeTurn = false;
	}
	
	private Path GetSpiralPath()
	{
		List<Vector2I> quadrants = new List<Vector2I>();
		
		Vector2I downRight = new Vector2I( 1,  1);
		Vector2I downLeft  = new Vector2I(-1,  1);
		Vector2I upRight   = new Vector2I( 1, -1);
		Vector2I upLeft    = new Vector2I(-1, -1);
		if(QuadrantIsValid(downRight)) quadrants.Add(downRight);
		if(QuadrantIsValid(downLeft)) quadrants.Add(downLeft);
		if(QuadrantIsValid(upRight)) quadrants.Add(upRight);
		if(QuadrantIsValid(upLeft)) quadrants.Add(upLeft);
		
		if(quadrants.Count == 0) return null;
		
		// Randomly select valid quadrant
		Vector2I quadrant = quadrants[Utils.RandomRange(quadrants.Count)];
		//GD.Print($"Quadrant: {quadrant}");
		// Get path cycle as offsets from CurrentCell
		Vector2I[] offsets = new Vector2I[] {
			new Vector2I(0, 0),
			new Vector2I(quadrant.X, 0),
			quadrant,
			new Vector2I(0, quadrant.Y)
		};
		
		// Randomize clockwise/counter-clockwise
		if(Utils.RandomRange(2) == 0)
		{
			Vector2I temp = offsets[1];
			offsets[1] = offsets[3];
			offsets[3] = temp;
		}
		
		// Build path
		int pathLength = Utils.RandomRange(SpiralLengthMin, SpiralLengthMax);
		List<Cell> path = new List<Cell>();
		for(int i = 0; i < pathLength; i++)
		{
			Vector2I offset = offsets[i % 4];
			Cell cell = CurrentRoom.CellMap[CurrentCell.Position() + offset];
			if(cell != CurrentCell && !WorldManager.TestTraversable(this, cell)) throw new Exception();
			path.Add(cell);
		}
		return new Pathfinding.Path(path);
	}
	private bool QuadrantIsValid(Vector2I direction)
	{
		if(!NeighborIsValid(new Vector2I(direction.X, 0))) return false;
		if(!NeighborIsValid(new Vector2I(0, direction.Y))) return false;
		if(!NeighborIsValid(direction)) return false;
		return true;
	}
	void QuadrantTest(Vector2I direction)
	{
		GD.Print(CurrentCell.Position() + new Vector2I(direction.X, 0));
		GD.Print(CurrentCell.Position() + new Vector2I(0, direction.Y));
		GD.Print(CurrentCell.Position() + direction);
	}
	private bool NeighborIsValid(Vector2I offset)
	{
		return CurrentRoom.CellMap.TryGetValue(CurrentCell.Position() + offset, out Cell neighbor) && CellIsValid(neighbor);
	}
	private bool CellIsValid(Cell cell)
	{
		return WorldManager.TestTraversable(this, cell);
	}
	
	public override bool IsMobile() => true;
}
