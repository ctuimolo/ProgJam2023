using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Spiders;

public partial class Spider : Enemy
{
	private bool NavigationInitialized = false;
	
	private BasicPathfinder Pathfinder;
	
	private Path CurrentPath;
	// Currently not used because path is remade each frame
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
		Vector2I direction = newCell.Position() - lastCell.Position();
		
		if(!MoveDirectionToAnimationName.TryGetValue(direction, out String name))
		{
			GD.Print($"no animation for {direction}");
			name = "walk_down";
		}
		_animationPlayer.Play(name);
		_animationWalker.Play(name);
		
		_animationWalker.Queue("idle");
	}
	
	Dictionary<Vector2I, String> MoveDirectionToAnimationName = new Dictionary<Vector2I, String>() {
		[new Vector2I( 0, -1)] = "walk_up",
		[new Vector2I( 1, -1)] = "walk_up_right",
		[new Vector2I( 1,  0)] = "walk_right",
		[new Vector2I( 1,  1)] = "walk_down_right",
		[new Vector2I( 0,  1)] = "walk_down",
		[new Vector2I(-1,  1)] = "walk_down_left",
		[new Vector2I(-1,  0)] = "walk_left",
		[new Vector2I(-1, -1)] = "walk_up_left",
	};
	
	public override bool IsMobile() => true;
}
