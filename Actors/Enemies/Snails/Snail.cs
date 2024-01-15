using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Snails;

public partial class Snail : Enemy
{
	
	[Export]
	public Sprite2D Sprite;
	
	public override void TakeTurn()
	{
		if(!_needsToTakeTurn)
		{
			return;
		}
		
		GridDirection direction = Utils.GetRandomMoveDirection(this);
		if(WorldManager.TryMoveActor(this, direction))
		{
			UpdateTurnDirection(direction);
		}
		
		_needsToTakeTurn = false;
	}
	
	private void UpdateTurnDirection(GridDirection direction)
	{
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
	
	private void SetTurnDirection(bool facingLeft)
	{
		Sprite.FlipH = !facingLeft;
	}
	
	public override bool IsMobile() => true;
}
