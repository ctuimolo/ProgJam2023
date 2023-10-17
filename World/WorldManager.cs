using Godot;
using System;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Player;


namespace ProgJam2023.World;

public partial class WorldManager : Node
{
    public enum State
    {
        Open,
        Busy,
    }

    public static Room CurrentRoom { get; private set; }
    public static Player CurrentPlayer { get; private set; }
    public static State CurrentState { get; private set; }

    private static int StepCoolDown = 18;
    private static int currStep     = 0;

    public static void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }

    public static void SetCurrentPlayer(Player player) 
    {
        CurrentPlayer = player;
    }

    public static void SpawnPlayer()
    {
        CurrentRoom.PutOnTile(CurrentRoom.StartingCell, CurrentPlayer);
    }

    public static void MakeBusy()
    {
        currStep = StepCoolDown;
        CurrentState = State.Busy;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CurrentState = State.Open;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (CurrentState == State.Busy)
        {
            if (currStep <= 0) 
            {
                currStep = 0;
                CurrentState = State.Open;
            } else
            {
                currStep--;
            }
        }
    }
}

