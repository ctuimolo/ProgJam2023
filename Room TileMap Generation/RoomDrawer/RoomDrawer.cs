using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomDrawer : Node2D
{
	[Export]
	private Node GDRoomDesigner;
	
	[Export]
	public TileMap TargetTileMap;
	[Export]
	public InstructionTileMap Instructions;
	
	[Export]
	public Resource NorthDoorPattern, SouthDoorPattern, EastDoorPattern, WestDoorPattern;
		
	public Door NorthDoor, SouthDoor, EastDoor, WestDoor;
	
	public class Door
	{
		public Resource DoorPattern;
		public Vector2I Position;
		public Vector2I EnterDirection => (Vector2I)DoorPattern.Get("enter_direction");
		public Vector2I EnterTile => Position - EnterDirection;
		public Vector2I PatternCenter => (Vector2I)DoorPattern.Get("center");
		public TileMapPattern TileMapPattern => (TileMapPattern)DoorPattern.Get("tile_map_pattern");
		
		public Door(Resource doorPattern)
		{
			DoorPattern = doorPattern;
		}
		public Door(Resource doorPattern, Vector2I position) : this(doorPattern)
		{
			Position = position;
		}
	}
	
	public Rect2I Rect { get; set; }
	public Vector2I BoundsMin => Rect.Position;
	public Vector2I BoundsMax => Rect.End - Vector2I.One;
	public Vector2I InsideMin => BoundsMin + new Vector2I(2, 3);
	public Vector2I InsideMax => BoundsMax - new Vector2I(2, 2);
	
	public Vector2I StartingCell { get; private set; }
	
	public RandomNumberGenerator RNG;
	
	
	public override void _Ready()
	{
		RNG = new RandomNumberGenerator();
	}
	
	public void DrawRoom()
	{
		DrawOuterWalls();
		InitializeDoors();
		DrawDoors();
		InitializeStartingCell();
		DrawDoorPaths();
	}
	
	private void DrawOuterWalls(string instruction = "black")
	{
		for(int x = BoundsMin.X; x < BoundsMax.X; x++)
		{
			Instructions.Draw(instruction, new Vector2I(x, BoundsMin.Y));
			Instructions.Draw(instruction, new Vector2I(x, BoundsMax.Y));
		}
		for(int y = BoundsMin.Y; y < BoundsMax.Y; y++)
		{
			Instructions.Draw(instruction, new Vector2I(BoundsMin.X, y));
			Instructions.Draw(instruction, new Vector2I(BoundsMax.X, y));
		}
		Instructions.Draw(instruction, BoundsMax);
	}
	
	private void DrawDoor(Door door)
	{
		TargetTileMap.SetPattern(0, door.Position - door.PatternCenter, door.TileMapPattern);
	}
	
	private void DrawDoors()
	{
		DrawDoor(NorthDoor);
		DrawDoor(SouthDoor);
		DrawDoor(EastDoor);
		DrawDoor(WestDoor);
	}
	
	private void DrawDoorPaths()
	{
		DrawPathDoorToPoint(NorthDoor, StartingCell);
		DrawPathDoorToPoint(SouthDoor, StartingCell);
		DrawPathDoorToPoint(EastDoor, StartingCell);
		DrawPathDoorToPoint(WestDoor, StartingCell);
	}
	
	private void DrawPathBetweenDoors(Door doorA, Door doorB)
	{
		Vector2I a = doorA.EnterTile;
		Instructions.Draw("floor", a);
		a -= doorA.EnterDirection;
		
		Vector2I b = doorB.EnterTile;
		Instructions.Draw("floor", b);
		b -= doorB.EnterDirection;
		
		DrawPath(a, b);
	}
	
	private void DrawPathDoorToPoint(Door door, Vector2I point)
	{
		Vector2I a = door.EnterTile;
		Instructions.Draw("floor", a);
		a -= door.EnterDirection;
		DrawPath(a, point);
	}
	
	private void DrawPath(Vector2I a, Vector2I b)
	{
		List<Vector2I> path = new SimplePathBuilder(RNG).GetPathBetweenBidirectional(a, b);
		foreach(Vector2I cell in path)
		{
			Instructions.Draw("floor", cell);
		}
	}
	
	private void InitializeStartingCell()
	{	
		Vector2I min = InsideMin + Rect.Size / 4;
		Vector2I max = InsideMax - Rect.Size / 4;
		StartingCell = new Vector2I(
			RNG.RandiRange(min.X, max.X),
			RNG.RandiRange(min.Y, max.Y)
		);
	}
	
	private void InitializeDoors()
	{
		NorthDoor = new Door(NorthDoorPattern);
		NorthDoor.Position = new Vector2I(
			RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
			BoundsMin.Y + 2
		);
		
		SouthDoor = new Door(SouthDoorPattern);
		SouthDoor.Position = new Vector2I(
			RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
			BoundsMax.Y - 1
		);
		
		EastDoor = new Door(EastDoorPattern);
		EastDoor.Position = new Vector2I(
			BoundsMax.X - 1,
			RNG.RandiRange(BoundsMin.Y + 3, BoundsMax.Y - 3)
		);
		
		WestDoor = new Door(WestDoorPattern);
		WestDoor.Position = new Vector2I(
			BoundsMin.X + 1,
			RNG.RandiRange(BoundsMin.Y + 3, BoundsMax.Y - 3)
		);
	}
	
}
