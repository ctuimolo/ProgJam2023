using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Moldmen;

public partial class Moldman : Enemy
{
	
	[Export]
	public Sprite2D Sprite;
	
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
		
		// Navigate to player
		Pathfinder.NavigationGraphBuilder.UpdateWeights();
		CurrentPath = Pathfinder.GetPath(CurrentCell, WorldManager.CurrentPlayer.CurrentCell);
		PathIndex = 1;
		
		// Move on path
		if(CurrentPath != null && PathIndex < CurrentPath.Count)
		{
			Vector2I vector = CurrentPath[PathIndex].Position() - CurrentCell.Position();
			GridDirection direction = Utils.VectorToDirection(vector);
			if(WorldManager.TryMoveActor(this, direction))
			{
				PathIndex++;
				
				UpdateTurnDirection(direction);
			}
		}
		
		_needsToTakeTurn = false;
	}
	
	private void UpdateTurnDirection(GridDirection direction)
	{
		// If this direction isn't left or right but following a path, update to face next turn direction
		if(direction != GridDirection.Right && direction != GridDirection.Left && CurrentPath != null)
		{
			direction = GetNextDirectionInPath(3);
		}
		
		// If direction is left or right, update to face that direction
		if(direction == GridDirection.Left)
		{
			SetTurnDirection(true);
		}
		else if(direction == GridDirection.Right)
		{
			SetTurnDirection(false);
		}
	}
	
	private GridDirection GetNextDirectionInPath(int maxNextSteps = -1)
	{
		if(maxNextSteps == -1)
		{
			maxNextSteps = int.MaxValue;
		}
		
		for(int i = PathIndex + 1; i < CurrentPath.Count; i++)
		{
			Vector2I difference = CurrentPath[i].Position() - CurrentPath[i - 1].Position();
			if(difference.X > 0)
			{
				return GridDirection.Right;
			}
			else if(difference.X < 0)
			{
				return GridDirection.Left;
			}
			
			maxNextSteps--;
			if(maxNextSteps <= 0) break;
		}
		
		return GridDirection.None;
	}
	
	private void SetTurnDirection(bool facingLeft)
	{
		Sprite.FlipH = facingLeft;
	}
	
	public override bool IsMobile() => true;
}
