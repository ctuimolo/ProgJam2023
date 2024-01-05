using Godot;
using System;

namespace ProgJam2023.World;

public partial class PlayState : PlayScene.State
{
	
	public PlayState(PlayScene stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		GD.Print("Gameplay start");
	}
	public override void Update()
	{

	}
	public override void Exit()
	{
		
	}
	
}
