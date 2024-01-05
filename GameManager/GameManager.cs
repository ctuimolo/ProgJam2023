using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.StateMachines;

namespace ProgJam2023.GameManagement;

public partial class GameManager : StateMachineNode
{

	public static GameManager Instance { get; private set; }
	
	// Scenes
	[Export]
	public PackedScene MainMenuScene;
	[Export]
	public PackedScene PlayScene;

	// States
	public MainMenuState MainMenuState;
	public PlayState PlayState;

	public override void _Ready()
	{
		if(Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;

		// Initialize states
		MainMenuState = new MainMenuState(this);
		PlayState = new PlayState(this);

		MoveToState(MainMenuState);
	}
	
	public void Quit()
	{
		GetTree().Quit();
	}

	public abstract class State : State<GameManager>
	{
		public State(GameManager stateMachine) : base(stateMachine) { }
	}
	
}
