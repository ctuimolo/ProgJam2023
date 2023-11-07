using Godot;
using System;

public partial class RoomTileMapGenerator : Node
{
	[Export]
	private Node GDRoomTileMapGenerator;
	
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
