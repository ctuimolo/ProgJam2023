using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class InstructionTileMap : Node
{
	
	[Export]
	private Node GDInstructionTileMap;
	
	public void Draw(string tileName, Vector2I cell)
	{
		GDInstructionTileMap.Call("draw", tileName, cell);
	}
	
	/*
	public void DrawArray(string tileName, Vector2I[] cells)
	{
		GDInstructionTileMap.Call("draw_array", tileName, cells);
	}
	*/
	
}
