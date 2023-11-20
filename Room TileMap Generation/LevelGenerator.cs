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
	
	
	public override void _Ready()
	{
		GenerateTestGrid(5, 5);
		RoomManager.DebugStartRoom = GetRoomName(new Vector2I(2, 2));
	}
	private void GenerateTestGrid(int width, int height)
	{
		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Vector2I coords = new Vector2I(x, y);
				RoomParameters parameters = new RoomParameters(DebugParameters);
				parameters.RoomName = GetRoomName(coords);
				parameters.HasNorthDoor = false;
				parameters.HasSouthDoor = false;
				parameters.HasEastDoor = false;
				parameters.HasWestDoor = false;
				if(y > 0)
				{
					parameters.HasNorthDoor = true;
					parameters.NorthRoom = GetRoomName(coords + new Vector2I(0, -1));
				}
				if(y < height - 1)
				{
					parameters.HasSouthDoor = true;
					parameters.SouthRoom = GetRoomName(coords + new Vector2I(0, 1));
				}
				if(x > 0)
				{
					parameters.HasWestDoor = true;
					parameters.WestRoom = GetRoomName(coords + new Vector2I(-1, 0));
				}
				if(x < width - 1)
				{
					parameters.HasEastDoor = true;
					parameters.EastRoom = GetRoomName(coords + new Vector2I(1, 0));
				}
				GenerateRoom(parameters);
			}
		}
	}
	private string GetRoomName(Vector2I coords)
	{
		return $"Room {coords}";
	}
	
	private void GenerateRoom(RoomParameters parameters)
	{
		RoomGenerator roomGenerator = (RoomGenerator)RoomGeneratorScene.Instantiate();
		AddChild(roomGenerator);
		roomGenerator.GenerationComplete += OnRoomGenerated;
		roomGenerator.Parameters = parameters;
		roomGenerator.Generate();
		GeneratingRoomsCount++;
	}
	
	private void OnRoomGenerated(RoomGenerator roomGenerator, Room room)
	{
		GeneratingRoomsCount--;
		roomGenerator.RemoveChild(room);
		string name = roomGenerator.Parameters.RoomName;
		RoomManager.AddRoom(name, room);
		//RoomManager.DebugStartRoom = name;
		
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
