using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.World;

namespace ProgJam2023.GameManagement;

public class PlayState : GameManager.State
{
	
	public PlayScene PlayScene;

	public PlayState(GameManager stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		// Enter main gameplay scene
		InstantiateScene();
	}
	public override void Update()
	{

	}
	public override void Exit()
	{
		// Remove main scene
	}
	
	private void InstantiateScene()
	{
		// Get PackedScene and instantiate
		PackedScene scene = StateMachine.PlayScene;
		PlayScene = scene.Instantiate() as PlayScene;
		GD.Print(PlayScene, " !!!!");
		// Add as child to GameManager
		StateMachine.AddChild(PlayScene);
	}
	private void DestroyScene()
	{
		StateMachine.RemoveChild(PlayScene);
	}

}
