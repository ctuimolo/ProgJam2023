using Godot;
using System;

public partial class RoomDesigner : Node2D
{
	[Export]
	private Node GDRoomDesigner;
	
	public Rect2I Rect {
		get => (Rect2I)GDRoomDesigner.Get("rect");
		set => GDRoomDesigner.Set("rect", value);
	}
	
	public Vector2I NorthDoorPosition => (Vector2I)GDRoomDesigner.Call("north_door_position");
	public Vector2I SouthDoorPosition => (Vector2I)GDRoomDesigner.Call("south_door_position");
	public Vector2I EastDoorPosition => (Vector2I)GDRoomDesigner.Call("east_door_position");
	public Vector2I WestDoorPosition => (Vector2I)GDRoomDesigner.Call("west_door_position");
	
	public Vector2I StartingCell => (Vector2I)GDRoomDesigner.Get("starting_cell");
	
	public void DesignRoom()
	{
		GDRoomDesigner.Call("design_room");
	}
	
}
