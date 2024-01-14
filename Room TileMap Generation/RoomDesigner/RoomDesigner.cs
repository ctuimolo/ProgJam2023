using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.RoomDesignParameters;
using ProgJam2023.RoomTileMapGeneration.Paths;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomDesigner : Node
{
	[Export]
	public TileMap TargetTileMap;
	[Export]
	public InstructionTileMap Instructions;
	
	[Export]
	public RoomTileMapEditor TileMapEditor;
	
	[Export]
	public int EnemySpawnAttempts = 10;
	
	public RoomPathDrawer PathDrawer;
	
	public Rect2I Rect { get; set; }
	public Vector2I BoundsMin => Rect.Position;
	public Vector2I BoundsMax => Rect.End - Vector2I.One;
	public Vector2I InsideMin => BoundsMin + new Vector2I(2, 3);
	public Vector2I InsideMax => BoundsMax - new Vector2I(2, 2);
	
	public Vector2I StartingCell { get; private set; }
	
	public struct EnemySpawn
	{
		public SpawnableEnemy Enemy;
		public Vector2I Cell;
	}
	public List<EnemySpawn> EnemySpawns = new List<EnemySpawn>();
	
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
	public RoomParametersCollapsed ParametersCollapsed;
	
	public void Reset()
	{
		Rect = new Rect2I(0, 0, 0, 0);
		StartingCell = new Vector2I(0, 0);
		EnemySpawns.Clear();
		NorthDoor = null;
		SouthDoor = null;
		EastDoor = null;
		WestDoor = null;
		TileMapEditor.Reset();
	}
	
	public void DesignRoom()
	{
		TileMapEditor.RNG = RNG;
		
		InitializeRect();
		DrawRoomOutline();
		
		InitializePathDrawer();
		
		InitializeDoors();
		DrawDoors();
		
		InitializeStartingCell();
		
		DrawPathsToDoors();
		
		InitializeEnemies();
	}
	
	private void InitializePathDrawer()
	{
		PathDrawer = new RoomPathDrawer(TileMapEditor, Rect, "PathFloor", "PathWall");
	}
	
	// Define size of room
	private void InitializeRect()
	{
		Vector2I size = ParametersCollapsed.Layout.Size;
		Rect = new Rect2I(-size / 2, size);
	}
	private void DrawRoomOutline()
	{
		TileMapEditor.DrawInstructionRectOutline(BoundsMin, BoundsMax, "black", "RoomOutline");
	}
	
	// Define door positions
	private void InitializeDoors()
	{
		if(Parameters.Data.HasNorthDoor)
		{
			NorthDoor = new Door(NorthDoorPattern);
			NorthDoor.Position = new Vector2I(
				RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
				BoundsMin.Y + 2
			);
		}
		
		if(Parameters.Data.HasSouthDoor)
		{
			SouthDoor = new Door(SouthDoorPattern);
			SouthDoor.Position = new Vector2I(
				RNG.RandiRange(BoundsMin.X + 3, BoundsMax.X - 3),
				BoundsMax.Y - 1
			);
		}
		
		if(Parameters.Data.HasEastDoor)
		{
			EastDoor = new Door(EastDoorPattern);
			EastDoor.Position = new Vector2I(
				BoundsMax.X - 1,
				RNG.RandiRange(BoundsMin.Y + 3, BoundsMax.Y - 3)
			);
		}
		
		if(Parameters.Data.HasWestDoor)
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
		TileMapEditor.DrawDoor(door, "Door");
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
		TileMapEditor.DrawInstruction(StartingCell, "floor", "StartingCell");
	}
	
	private void InitializeEnemies()
	{
		Vector2I min = InsideMin + Rect.Size / 3;
		Vector2I max = InsideMax - Rect.Size / 3;
		foreach(SpawnableEnemy enemy in ParametersCollapsed.Enemies.Enemies)
		{
			bool success = false;
			for(int i = 0; i < EnemySpawnAttempts && !success; i++)
			{
				success = TrySpawnEnemy(enemy, min, max);
			}
		}
	}
	private bool TrySpawnEnemy(SpawnableEnemy enemy, Vector2I min, Vector2I max)
	{
		Vector2I cell = new Vector2I(
			RNG.RandiRange(min.X, max.X),
			RNG.RandiRange(min.Y, max.Y)
		);
		if(!enemy.CanSpawn(cell, TileMapEditor))
		{
			return false;
		}
		AddEnemy(enemy, cell);
		return true;
	}
	private void AddEnemy(SpawnableEnemy enemy, Vector2I cell)
	{
		enemy.PrepareSpawnArea(cell, TileMapEditor);
		EnemySpawn spawn = new EnemySpawn() {
			Enemy = enemy,
			Cell = cell
		};
		EnemySpawns.Add(spawn);
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
		Vector2I doorExitCell = door.EnterTile;
		TileMapEditor.DrawInstruction(doorExitCell, "floor", "Door");
		doorExitCell -= door.EnterDirection;
		PathDrawer.DrawPath(doorExitCell, StartingCell, 1, false, PathCombineMode.Union);
	}
	
}
