using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomWFCManager : Node
{
	[Export]
	private Node GDRoomWFCManager;
	
	public TileMap TargetTileMap => (TileMap)GDRoomWFCManager.Get("target_tile_map");
	
	[Signal]
	public delegate void TilesFinishedEventHandler();
	
	public void Collapse()
	{
		GDRoomWFCManager.Call("collapse");
	}
	
	public void SetRect(Rect2I rect)
	{
		GDRoomWFCManager.Call("set_rect", rect);
	}
	
	private void OnTilesFinished()
	{
		EmitSignal(SignalName.TilesFinished);
	}
}
