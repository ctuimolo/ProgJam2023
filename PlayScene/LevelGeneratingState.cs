using Godot;
using System;

namespace ProgJam2023.World;

public partial class LevelGeneratingState : PlayScene.State
{
	
	public LevelGeneratingState(PlayScene stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		StateMachine.LevelGenerator.LevelGenerated += OnLevelGenerated;
		StateMachine.LevelGenerator.GenerateLevel();
	}
	public override void Update()
	{

	}
	public override void Exit()
	{
		
	}
	
	private void OnLevelGenerated()
	{
		StateMachine.MoveToState(StateMachine.PlayState);
	}
	
}
