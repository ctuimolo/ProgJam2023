using Godot;
using System;

using ProgJam2023.World;

namespace ProgJam2023.Actors.Player;

public partial class Player : GridActor
{
    [Export]
    protected new int _stepSpeed = 18; // Speed is framecount of total animation, not velocity (smaller=faster)

    // Probably to be called in Godot scene room's _Ready function
    protected void InitPlayer()
	{
        _animationWalker.SpeedScale = 1.5f;

		WorldManager.SetCurrentPlayer(this);
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		InitPlayer();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override async void _Process(double delta)
	{
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (WorldManager.CurrentState == WorldManager.State.Open)
        {
            Direction direction = Utils.VectorToDirection(Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Round());

            if (direction != Direction.None)
            {
                WorldManager.MoveActor(this, direction);
            }
        }
    }
}
