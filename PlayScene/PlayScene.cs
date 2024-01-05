using Godot;
using System;

using ProgJam2023.StateMachines;
using ProgJam2023.Rooms;
using ProgJam2023.RoomTileMapGeneration;

namespace ProgJam2023.World;

public partial class PlayScene : StateMachineNode
{
	public static PlayScene Instance { get; private set; }
	
	[Export]
	public WorldManager WorldManager;
	[Export]
	public LevelGenerator LevelGenerator;
	[Export]
	public RoomManager RoomManager;
	
	public LevelGeneratingState LevelGeneratingState;
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
		LevelGeneratingState = new LevelGeneratingState(this);
		PlayState = new PlayState(this);

		MoveToState(LevelGeneratingState);
	}
	
	public abstract class State : State<PlayScene>
	{
		public State(PlayScene stateMachine) : base(stateMachine) { }
	}
}
