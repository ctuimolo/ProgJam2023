using Godot;
using System;

public partial class RoomGenerator : Node
{
	[Export]
	private ProgJam2023.Rooms.Room Room;
	
	[Export]
	public RoomDesigner RoomDesigner; // make private later
	
	[Export]
	private RoomTileMapGenerator RoomTileMapGenerator;
	
	[Export]
	private Node TileMapUnflattener;
	
	[Signal]
	public delegate void GenerationCompleteEventHandler(ProgJam2023.Rooms.Room room);
	
	private bool Generated;
	
	public override void _Ready()
	{
		RoomTileMapGenerator.TilesFinished += OnTilesFinished;
	}
	
	public void Generate()
	{
		if(Generated)
		{
			throw new InvalidOperationException();
		}
		
		RoomDesigner.DesignRoom();
		RoomTileMapGenerator.SetRect(RoomDesigner.Rect);
		RoomTileMapGenerator.Collapse();
	}
	
	private void OnTilesFinished()
	{
		TileMapUnflattener.Call("unflatten");
		
		Room.StartingCell = RoomDesigner.StartingCell;
		
		// Add doors, items, enemies, etc.
		
		EmitSignal(SignalName.GenerationComplete, Room);
	}
}