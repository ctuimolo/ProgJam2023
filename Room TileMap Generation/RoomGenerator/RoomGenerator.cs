using Godot;
using System;

using ProgJam2023;
using ProgJam2023.Rooms;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomGenerator : Node
{
	[Export]
	private ProgJam2023.Rooms.Room Room;
	
	[Export]
	private PackedScene DoorScene;
	
	[Export]
	public RoomDesigner Designer;
	
	[Export]
	private RoomTileMapGenerator RoomTileMapGenerator;
	
	[Export]
	private Node TileMapUnflattener;
	
	[Signal]
	public delegate void GenerationCompleteEventHandler(RoomGenerator roomGenerator, Room room);
	
	public RoomParameters Parameters = null;
	
	private RandomNumberGenerator RNG;
	
	private bool Generated = false;
	
	public override void _Ready()
	{	
		RNG = new RandomNumberGenerator();
		RoomTileMapGenerator.TilesFinished += OnTilesFinished;
	}
	
	public void Generate()
	{
		if(Generated)
		{
			throw new InvalidOperationException();
		}
		
		Parameters.Collapse(RNG);
		
		GD.Print("Parameters: ", Parameters);
		
		Designer.RNG = RNG;
		Designer.Parameters = Parameters;
		Designer.DesignRoom();
		
		RoomTileMapGenerator.SetRect(Designer.Rect);
		RoomTileMapGenerator.Collapse();
	}
	
	private void OnTilesFinished()
	{
		Generated = true;
		
		RoomTileMapGenerator.TargetTileMap.Hide();
		TileMapUnflattener.Call("unflatten");
		
		Room.Name = Parameters.RoomName;
		Room.StartingCell = Designer.StartingCell;
		AddDoors();
		
		// Add doors, items, enemies, etc.
		
		EmitSignal(SignalName.GenerationComplete, this, Room);
	}
	
	private void AddDoors()
	{
		if(Parameters.HasNorthDoor)
		{
			AddDoor(Designer.NorthDoor, Parameters.NorthRoom);
		}
		if(Parameters.HasSouthDoor)
		{
			AddDoor(Designer.SouthDoor, Parameters.SouthRoom);
		}
		if(Parameters.HasEastDoor)
		{
			AddDoor(Designer.EastDoor, Parameters.EastRoom);
		}
		if(Parameters.HasWestDoor)
		{
			AddDoor(Designer.WestDoor, Parameters.WestRoom);
		}
	}
	private void AddDoor(RoomDesigner.Door door, string toRoom)
	{
		AddDoor(door.Position, door.EnterDirection, toRoom);
	}
	private void AddDoor(Vector2I cell, Vector2I direction, string toRoom)
	{
		Door door = (Door)DoorScene.Instantiate();
		door.Name = DirectionToString(direction) + " Door";
		door.Position = 16 * cell;
		door.ToRoom = toRoom;
		door.ToDoor = DirectionToString(-direction) + " Door";
		door.ExitDirection = Utils.VectorToDirection(direction);
		Room.Map.AddChild(door);
	}
	private string DirectionToString(Vector2I direction)
	{
		if(direction == new Vector2I(0, -1))
		{
			return "North";
		}
		else if(direction == new Vector2I(0, 1))
		{
			return "South";
		}
		else if(direction == new Vector2I(1, 0))
		{
			return "East";
		}
		else if(direction == new Vector2I(-1, 0))
		{
			return "West";
		}
		throw new ArgumentException();
	}
}
