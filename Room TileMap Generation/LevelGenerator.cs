using Godot;
using System;

using ProgJam2023.Rooms;
using ProgJam2023.RoomDesignParameters;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class LevelGenerator : Node
{
	[Export]
	public RoomManager RoomManager;
	
	[Export]
	public PackedScene RoomGeneratorScene;
	
	[Export]
	public RoomParameters DebugParameters;
	
	[Export]
	public PackedScene EnemyTestRoom;
	[Export]
	public bool LoadEnemyTestRoom = false;
	
	[Export]
	public PackedScene RoomScene;
	
	
	private int GeneratingRoomsCount;
	
	
	public override void _Ready()
	{
		if(LoadEnemyTestRoom)
		{
			LoadTestRoom("Enemy TestRoom", EnemyTestRoom);
			return;
		}
		
		GenerateTestGrid(5, 5);
		RoomManager.DebugStartRoom = GetRoomName(new Vector2I(2, 2));
	}
	
	private void LoadTestRoom(String name, PackedScene scene)
	{
		Room room = RoomScene.Instantiate<Room>();
		RoomTileMap tileMap = scene.Instantiate<RoomTileMap>();
		room.AddChild(tileMap);
		room.Map = tileMap;
		
		RoomManager.AddRoom(name, room);
		RoomManager.DebugStartRoom = name;
		
		RoomManager.Initialize();
	}
	
	private void GenerateTestGrid(int width, int height)
	{
		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Vector2I coords = new Vector2I(x, y);
				RoomParameters parameters = new RoomParameters(DebugParameters);
				parameters.Data = new RoomData();
				RoomData data = parameters.Data;
				data.RoomName = GetRoomName(coords);
				data.HasNorthDoor = false;
				data.HasSouthDoor = false;
				data.HasEastDoor = false;
				data.HasWestDoor = false;
				if(y > 0)
				{
					data.HasNorthDoor = true;
					data.NorthRoom = GetRoomName(coords + new Vector2I(0, -1));
				}
				if(y < height - 1)
				{
					data.HasSouthDoor = true;
					data.SouthRoom = GetRoomName(coords + new Vector2I(0, 1));
				}
				if(x > 0)
				{
					data.HasWestDoor = true;
					data.WestRoom = GetRoomName(coords + new Vector2I(-1, 0));
				}
				if(x < width - 1)
				{
					data.HasEastDoor = true;
					data.EastRoom = GetRoomName(coords + new Vector2I(1, 0));
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
		string name = roomGenerator.Parameters.Data.RoomName;
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
