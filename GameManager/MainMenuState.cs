using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.UI.MainMenu;

namespace ProgJam2023.GameManagement;

public class MainMenuState : GameManager.State
{
	
	public MainMenu MainMenu;

	public MainMenuState(GameManager stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		// Enter main menu scene
		InstantiateScene();
		MainMenu.Play += StartGame;
		MainMenu.Quit += QuitGame;
	}
	public override void Update()
	{

	}
	public override void Exit()
	{
		// Remove main menu scene
		DestroyScene();
	}
	
	
	private void InstantiateScene()
	{
		// Get PackedScene and instantiate
		PackedScene scene = StateMachine.MainMenuScene;
		MainMenu = scene.Instantiate() as MainMenu;
		// Add as child to GameManager
		StateMachine.AddChild(MainMenu);
	}
	private void DestroyScene()
	{
		StateMachine.RemoveChild(MainMenu);
	}
	
	
	public void StartGame()
	{
		StateMachine.MoveToState(StateMachine.PlayState);
	}
	
	public void QuitGame()
	{
		StateMachine.Quit();
	}

}
