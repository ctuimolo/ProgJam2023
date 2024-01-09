using Godot;
using System;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Spiders;

public partial class Spider : Enemy
{
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
		SpiderPathingEvaluator pathingEvaluator = new SpiderPathingEvaluator(CurrentRoom);
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
		
		Pathfinder.NavigationGraphBuilder.UpdateWeights();
		
		if(true || CurrentPath == null || PathIndex >= CurrentPath.Count)
		{
			CurrentPath = Pathfinder.GetPath(CurrentCell, WorldManager.CurrentPlayer.CurrentCell);
			PathIndex = 1;
		}
		
		if(CurrentPath != null && PathIndex < CurrentPath.Count)
		{
			Cell lastCell = CurrentCell;
			Cell nextCell = CurrentPath[PathIndex];
			if(WorldManager.TryMoveActorToCell(this, nextCell))
			{
				MoveAnimation(lastCell, CurrentCell);
				PathIndex++;
			}
		}
		
		_needsToTakeTurn = false;
	}
	
	private void MoveAnimation(Cell lastCell, Cell newCell)
	{
		///////////////////////////////
		CurrentRoom.UpdateCellPositionDraw(this);
		State = GridActor.ActorState.Idle;
	}
	
	public override bool IsMobile() => true;
}
