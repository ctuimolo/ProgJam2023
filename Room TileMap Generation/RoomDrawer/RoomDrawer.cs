using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.TileHelper;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomDrawer : Node2D
{
	
	[Export]
	public TileMap TargetTileMap;
	[Export]
	public InstructionTileMap Instructions;
	
	public RandomNumberGenerator RNG;
	
	public override void _Ready()
	{
		RNG = new RandomNumberGenerator();
	}
	
	public bool CellIsNavigable(Vector2I cell)
	{
		Tile tile = Tile.FromTileMap(TargetTileMap, 0, cell);
		if(tile != null)
		{
			return false;
		}
		
		string instruction = Instructions.Read(cell);
		return instruction != "wall" && instruction != "black";
	}
	
	public void DrawRectOutline(Rect2I rect, string instruction)
	{
		Vector2I min = rect.Position;
		Vector2I max = rect.End;
		DrawRectOutline(min, max, instruction);
	}
	public void DrawRectOutline(Vector2I min, Vector2I max, string instruction)
	{
		for(int x = min.X; x < max.X; x++)
		{
			Instructions.Draw(instruction, new Vector2I(x, min.Y));
			Instructions.Draw(instruction, new Vector2I(x, max.Y));
		}
		for(int y = min.Y; y < max.Y; y++)
		{
			Instructions.Draw(instruction, new Vector2I(min.X, y));
			Instructions.Draw(instruction, new Vector2I(max.X, y));
		}
		Instructions.Draw(instruction, max);
	}
	
	public void DrawDoor(RoomDesigner.Door door)
	{
		TargetTileMap.SetPattern(0, door.Position - door.PatternCenter, door.TileMapPattern);
	}
	
	public void DrawPathBetweenDoors(RoomDesigner.Door doorA, RoomDesigner.Door doorB)
	{
		Vector2I a = doorA.EnterTile;
		Instructions.Draw("floor", a);
		a -= doorA.EnterDirection;
		
		Vector2I b = doorB.EnterTile;
		Instructions.Draw("floor", b);
		b -= doorB.EnterDirection;
		
		DrawPath(a, b);
	}
	
	public void DrawPathDoorToPoint(RoomDesigner.Door door, Vector2I point)
	{
		Vector2I a = door.EnterTile;
		Instructions.Draw("floor", a);
		a -= door.EnterDirection;
		DrawPath(a, point);
	}
	
	public void DrawPath(Vector2I a, Vector2I b)
	{
		List<Vector2I> path = new SimplePathBuilder(RNG).GetPathBetweenBidirectional(a, b);
		foreach(Vector2I cell in path)
		{
			Instructions.Draw("floor", cell);
		}
	}
	
}
