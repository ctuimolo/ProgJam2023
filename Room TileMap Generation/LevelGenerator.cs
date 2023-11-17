using Godot;
using System;

using ProgJam2023.Rooms;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class LevelGenerator : Node
{
	[Export]
	public RoomManager RoomManager;
	
	[Export]
	public RoomGenerator RoomGenerator;
	
	[Export]
	public Vector2I RoomSizeMin;
	[Export]
	public Vector2I RoomSizeMax;
	
	private RandomNumberGenerator rng;
	
	private Rect2I GetRandomRoomSize()
	{
		int x = rng.RandiRange(RoomSizeMin.X, RoomSizeMax.X);
		int y = rng.RandiRange(RoomSizeMin.Y, RoomSizeMax.Y);
		Vector2I size = new Vector2I(x, y);
		return new Rect2I(-size / 2, size);
	}
	
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
		RoomGenerator.Drawer.Rect = GetRandomRoomSize();
		
		RoomGenerator.GenerationComplete += OnRoomGenerated;
		RoomGenerator.Generate();
	}
	
	private void OnRoomGenerated(Room room)
	{
		RoomGenerator.RemoveChild(room);
		RoomManager.AddRoom("generated room", room);
		RoomManager.DebugStartRoom = "generated room";
		RoomManager.Initialize();
	}
}
