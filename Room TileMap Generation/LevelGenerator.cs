using Godot;
using System;

using ProgJam2023.Rooms;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class LevelGenerator : Node
{
	[Export]
	public RoomManager RoomManager;
	
	[Export]
	public PackedScene RoomGeneratorScene;
	
	[Export]
	public RoomParameters DebugParameters;
	
	
	private int GeneratingRoomsCount;
	
	
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
		
		GenerateRoom(DebugParameters);
		//GenerateRoom(DebugParameters);
	}
	
	private void GenerateRoom(RoomParameters parameters)
	{
		RoomGenerator roomGenerator = (RoomGenerator)RoomGeneratorScene.Instantiate();
		AddChild(roomGenerator);
		roomGenerator.GenerationComplete += OnRoomGenerated;
		roomGenerator.Parameters = new RoomParameters(parameters);
		roomGenerator.Generate();
		GeneratingRoomsCount++;
	}
	
	private void OnRoomGenerated(RoomGenerator roomGenerator, Room room)
	{
		GeneratingRoomsCount--;
		roomGenerator.RemoveChild(room);
		string name = $"generated room {GeneratingRoomsCount}";
		RoomManager.AddRoom(name, room);
		RoomManager.DebugStartRoom = name;
		
		if(GeneratingRoomsCount == 0)
		{
			AllRoomsGenerated();
		}
	}
	
	private void AllRoomsGenerated()
	{
		RoomManager.Initialize();
	}
}
