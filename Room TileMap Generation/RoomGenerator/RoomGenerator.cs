using Godot;
using System;

using ProgJam2023;
using ProgJam2023.Rooms;
using ProgJam2023.RoomDesignParameters;

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
	private RoomWFCManager RoomWFCManager;
	
	[Export]
	private Node TileMapUnflattener;
	
	[Signal]
	public delegate void GenerationCompleteEventHandler(RoomGenerator roomGenerator, Room room);
	
	public RoomParameters Parameters = null;
	public RoomParametersCollapsed ParametersCollapsed = null;
	
	private RandomNumberGenerator RNG;
	
	private bool Generated = false;
	
	public override void _Ready()
	{
		RNG = new RandomNumberGenerator();
		RoomWFCManager.TilesFinished += OnTilesFinished;
	}
	
	public void Generate()
	{
		if(Generated)
		{
			throw new InvalidOperationException();
		}
		
		ParametersCollapsed = Parameters.GetCollapsed(RNG);
		
		GD.Print("ParametersCollapsed: ", ParametersCollapsed);
		
		Designer.RNG = RNG;
		Designer.Parameters = Parameters;
		Designer.ParametersCollapsed = ParametersCollapsed;
		Designer.DesignRoom();
		
		RoomWFCManager.SetRect(Designer.Rect);
		RoomWFCManager.Collapse();
	}
	
	private void OnTilesFinished()
	{
		Generated = true;
		
		RoomWFCManager.TargetTileMap.Hide();
		TileMapUnflattener.Call("unflatten");
		
		Room.Name = Parameters.Data.RoomName;
		Room.StartingCell = Designer.StartingCell;
		AddDoors();
		AddEnemies();
		
		EmitSignal(SignalName.GenerationComplete, this, Room);
	}
	
	private void AddDoors()
	{
		if(Parameters.Data.HasNorthDoor)
		{
			AddDoor(Designer.NorthDoor, Parameters.Data.NorthRoom);
		}
		if(Parameters.Data.HasSouthDoor)
		{
			AddDoor(Designer.SouthDoor, Parameters.Data.SouthRoom);
		}
		if(Parameters.Data.HasEastDoor)
		{
			AddDoor(Designer.EastDoor, Parameters.Data.EastRoom);
		}
		if(Parameters.Data.HasWestDoor)
		{
			AddDoor(Designer.WestDoor, Parameters.Data.WestRoom);
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
	
	private void AddEnemies()
	{
		foreach(RoomDesigner.EnemySpawn spawn in Designer.EnemySpawns)
		{
			EnemySpawner.Instruction instruction = new EnemySpawner.Instruction() {
				Type = spawn.Enemy.Type,
				Cell = spawn.Cell,
				Name = "Bingus"
			};
			Room.AddEnemySpawnInstruction(instruction);
		}
	}
	
	public struct Instruction
   {
      public EnemyType Type;
      public Vector2I Cell;
      public StringName Name;
   }
   public enum EnemyType
   {
      GreenSlime,
      PlaceHolder,
   }
}
