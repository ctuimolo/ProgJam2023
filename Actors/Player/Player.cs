using Godot;
using System;

using ProgJam2023.World;

namespace ProgJam2023.Actors.Player;

public partial class Player : GridActor
{
    // Probably to be called in Godot scene room's _Ready function
    protected void InitPlayer()
	{
		WorldManager.SetCurrentPlayer(this);
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		InitPlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
