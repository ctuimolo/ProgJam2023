using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomDesigner : Node
{
	[Export]
	public TileMap TargetTileMap;
	[Export]
	public InstructionTileMap Instructions;
	
	[Export]
	public RoomDrawer Drawer;
	
	public Rect2I Rect { get; set; }
	public Vector2I BoundsMin => Rect.Position;
	public Vector2I BoundsMax => Rect.End - Vector2I.One;
	public Vector2I InsideMin => BoundsMin + new Vector2I(2, 3);
	public Vector2I InsideMax => BoundsMax - new Vector2I(2, 2);
	
	public Vector2I StartingCell { get; private set; }
	
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
	
	public RandomNumberGenerator RNG;
	
	public RoomParameters Parameters;
	
	public void DesignRoom()
	{
		Drawer.RNG = RNG;
		
		InitializeRect();
		DrawRoomOutline();
		
		InitializeDoors();
		DrawDoors();
		
		InitializeStartingCell();
		
		DrawPathsToDoors();
	}
	
	// Define size of room
	private void InitializeRect()
	{
		int width = Parameters.GetSize(Parameters.Width).X;
		int height = Parameters.GetSize(Parameters.Height).Y;
		if(Parameters.IsSquareProportions())
		{
			width = Math.Min(width, height);
			height = width;
		}
		Vector2I size = new Vector2I(width, height);
		Rect = new Rect2I(-size / 2, size);
	}
	private void DrawRoomOutline()
	{
		Drawer.DrawRectOutline(BoundsMin, BoundsMax, "black");
	}
	
	// Define door positions
	private void InitializeDoors()
	{
		if(Parameters.HasNorthDoor)
		{
			NorthDoor = new Door(NorthDoorPattern);
			NorthDoor.Position = new Vector2I(
				RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
				BoundsMin.Y + 2
			);
		}
		
		if(Parameters.HasSouthDoor)
		{
			SouthDoor = new Door(SouthDoorPattern);
			SouthDoor.Position = new Vector2I(
				RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
				BoundsMax.Y - 1
			);
		}
		
		if(Parameters.HasEastDoor)
		{
			EastDoor = new Door(EastDoorPattern);
			EastDoor.Position = new Vector2I(
				BoundsMax.X - 1,
				RNG.RandiRange(BoundsMin.Y + 3, BoundsMax.Y - 3)
			);
		}
		
		if(Parameters.HasWestDoor)
		{
			WestDoor = new Door(WestDoorPattern);
			WestDoor.Position = new Vector2I(
				BoundsMin.X + 1,
				RNG.RandiRange(BoundsMin.Y + 3, BoundsMax.Y - 3)
			);
		}
	}
	private void DrawDoors()
	{
		DrawDoor(NorthDoor);
		DrawDoor(SouthDoor);
		DrawDoor(EastDoor);
		DrawDoor(WestDoor);
	}
	private void DrawDoor(Door door)
	{
		if(door == null)
		{
			return;
		}
		Drawer.DrawDoor(door);
	}
	
	// Define starting cell coords
	private void InitializeStartingCell()
	{	
		Vector2I min = InsideMin + Rect.Size / 4;
		Vector2I max = InsideMax - Rect.Size / 4;
		StartingCell = new Vector2I(
			RNG.RandiRange(min.X, max.X),
			RNG.RandiRange(min.Y, max.Y)
		);
	}
	
	private void DrawPathsToDoors()
	{
		DrawPathToDoor(NorthDoor);
		DrawPathToDoor(SouthDoor);
		DrawPathToDoor(EastDoor);
		DrawPathToDoor(WestDoor);
	}
	private void DrawPathToDoor(Door door)
	{
		if(door == null)
		{
			return;
		}
		Drawer.DrawPathDoorToPoint(door, StartingCell);
	}
	
}
