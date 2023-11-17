using Godot;
using System;

using ProgJam2023.Rooms;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomGenerator : Node
{
	[Export]
	private ProgJam2023.Rooms.Room Room;
	
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
		
		Room.StartingCell = Designer.StartingCell;
		
		// Add doors, items, enemies, etc.
		
		EmitSignal(SignalName.GenerationComplete, this, Room);
	}
}
