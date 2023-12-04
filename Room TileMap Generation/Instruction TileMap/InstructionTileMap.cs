using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class InstructionTileMap : Node
{
	
	[Export]
	public Node GDInstructionTileMap;
	
	public void Draw(string tileName, Vector2I cell)
	{
		GDInstructionTileMap.Call("draw", tileName, cell);
	}
	
	public string Read(Vector2I cell)
	{
		return (string)GDInstructionTileMap.Call("read", cell);
	}
	
	public void Clear()
	{
		GDInstructionTileMap.Call("clear");
	}
	
	/*
	public void DrawArray(string tileName, Vector2I[] cells)
	{
		GDInstructionTileMap.Call("draw_array", tileName, cells);
	}
	*/
	
}
