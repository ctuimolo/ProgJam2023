using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomTileMapGenerator : Node
{
	[Export]
	private Node GDRoomTileMapGenerator;
	
	public TileMap TargetTileMap => (TileMap)GDRoomTileMapGenerator.Get("target_tile_map");
	
	[Signal]
	public delegate void TilesFinishedEventHandler();
	
	public void Collapse()
	{
		GDRoomTileMapGenerator.Call("collapse");
	}
	
	public void SetRect(Rect2I rect)
	{
		GDRoomTileMapGenerator.Call("set_rect", rect);
	}
	
	private void OnTilesFinished()
	{
		EmitSignal(SignalName.TilesFinished);
	}
}
