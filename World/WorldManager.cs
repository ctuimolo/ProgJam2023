using Godot;
using System;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Player;


namespace ProgJam2023.World;

public static class WorldManager 
{
	public static Room CurrentRoom { get; private set; }
	public static Player CurrentPlayer { get; private set; }

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
}
