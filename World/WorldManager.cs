using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Player;
using ProgJam2023.Actors;

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

    static List<GridActor> _worldActors;

    public static void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }

    public static void SetCurrentPlayer(Player player) 
    {
        CurrentPlayer = player;
        AddActorToWorld(player);
    }

    public static void AddActorToWorld(GridActor actor)
    {
        if (_worldActors.Contains(actor)) 
        {
            return;
        }

        _worldActors.Add(actor);
    }

    public static void SpawnPlayer()
    {
        CurrentRoom.PutOnTile(CurrentRoom.StartingCell, CurrentPlayer);
    }

    public static void InitWorld()
    {
        _worldActors = new List<GridActor>();
        CurrentState = State.Open;
    }

    public static void MoveActor(GridActor actor, Direction direction)
    {
        if (actor.State != GridActor.ActorState.Idle) return;

        Vector2I toTile = actor.CurrentCell + Utils.DirectionToVector(direction);

        if (!CurrentRoom.TileMap.GetUsedCells(0).Contains(toTile)) return;

        actor.SingleStep(direction);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitWorld();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

