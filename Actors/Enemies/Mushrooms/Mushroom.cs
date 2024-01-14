using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;
using ProgJam2023.Rooms;
using ProgJam2023.Pathfinding;

namespace ProgJam2023.Actors.Enemies.Mushrooms;

public partial class Mushroom : Enemy
{
	[Export]
	public int AttackInterval = 3;
	
	int TurnCounter;
	
	public override void _Ready()
	{
		base._Ready();
		
		// Attack on first turn
		TurnCounter = AttackInterval;
	}
	
	public override void TakeTurn()
	{
		if(!_needsToTakeTurn)
		{
			return;
		}
		
		TurnCounter++;
		if(TurnCounter >= AttackInterval)
		{
			TurnCounter = 0;
			ShootSpores();
		}
		
		_needsToTakeTurn = false;
	}
	
	private void ShootSpores()
	{
		_animationPlayer.Play("attack");
		_animationPlayer.Queue("idle");
		///////// Animate spores effect
		///////// Damage Player and Enemies in nearby area
	}
	
	public override bool IsMobile() => false;
}
