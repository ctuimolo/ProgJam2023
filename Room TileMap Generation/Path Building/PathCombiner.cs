using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

[Flags]
public enum PathCombineMode {
	None = 0,
	FloorsOverlapFloors = 1,
	FloorsOverlapWalls = 2,
	WallsOverlapFloors = 4,
	WallsOverlapWalls = 8,
	WriteOverFloors = 16,
	WriteOverWalls = 32,
	OverlapFloors = FloorsOverlapFloors | WallsOverlapFloors,
	OverlapWalls = FloorsOverlapWalls | WallsOverlapWalls,
	OverlapAll = OverlapFloors | OverlapWalls,
	WriteOverAll = WriteOverFloors | WriteOverWalls,
	Union = OverlapAll | WriteOverWalls,
	Combine = FloorsOverlapFloors | WallsOverlapWalls,
	Overwrite = OverlapAll | WriteOverAll,
}

public class PathCombiner
{
	public HashSet<StringName> FloorIDs;
	public HashSet<StringName> WallIDs;
	
	public HashSet<StringName> FloorOverlaps = new HashSet<StringName>();
	public HashSet<StringName> WallOverlaps = new HashSet<StringName>();
	
	public PathCombiner()
	{
		FloorIDs = new HashSet<StringName>();
		WallIDs = new HashSet<StringName>();
	}
	public PathCombiner(StringName floor, StringName wall)
	{
		FloorIDs = new HashSet<StringName>(){ floor };
		WallIDs = new HashSet<StringName>(){ wall };
	}
	public PathCombiner(IEnumerable<StringName> floorIDs, IEnumerable<StringName> wallIDs)
	{
		FloorIDs = new HashSet<StringName>(floorIDs);
		WallIDs = new HashSet<StringName>(wallIDs);
	}
	
	public bool IsFloor(StringName claimID) => FloorIDs.Contains(claimID);
	public bool IsWall(StringName claimID) => WallIDs.Contains(claimID);
	
	// Check if a floor area in the new path can be on a cell with the claim ID
	public bool FloorCanOverlap(StringName claimID, PathCombineMode mode)
	{
		if(claimID == null) return true;
		
		if(IsFloor(claimID))
		{
			return mode.HasFlag(PathCombineMode.FloorsOverlapFloors);
		}
		else if(IsWall(claimID))
		{
			return mode.HasFlag(PathCombineMode.FloorsOverlapWalls);
		}
		else
		{
			return FloorOverlaps.Contains(claimID);
		}
	}
	
	// Check if a wall area in the new path can be on a cell with the claim ID
	public bool WallCanOverlap(StringName claimID, PathCombineMode mode)
	{
		if(claimID == null) return true;
		
		if(IsFloor(claimID))
		{
			return mode.HasFlag(PathCombineMode.WallsOverlapFloors);
		}
		else if(IsWall(claimID))
		{
			return mode.HasFlag(PathCombineMode.WallsOverlapWalls);
		}
		else
		{
			return WallOverlaps.Contains(claimID);
		}
	}
	
	// Check if the new path should write over the existing tile on the cell
	public bool CanWriteOver(StringName claimID, PathCombineMode mode)
	{
		if(claimID == null) return true;
		
		if(IsFloor(claimID))
		{
			return mode.HasFlag(PathCombineMode.WriteOverFloors);
		}
		else if(IsWall(claimID))
		{
			return mode.HasFlag(PathCombineMode.WriteOverWalls);
		}
		else
		{
			return false;
		}
	}
	
	public void RemoveNonOverwriteCells(CellPath path, PathCombineMode mode, RoomTileMapEditor editor)
	{
		HashSet<Vector2I> newFloors = new HashSet<Vector2I>();
		foreach(Vector2I cell in path.Floors)
		{
			StringName claimID = editor.ClaimedCellCollection.GetClaimID(cell);
			if(CanWriteOver(claimID, mode))
			{
				newFloors.Add(cell);
			}
		}
		HashSet<Vector2I> newWalls = new HashSet<Vector2I>();
		foreach(Vector2I cell in path.Walls)
		{
			StringName claimID = editor.ClaimedCellCollection.GetClaimID(cell);
			if(CanWriteOver(claimID, mode))
			{
				newWalls.Add(cell);
			}
		}
		path.Floors = newFloors;
		path.Walls = newWalls;
	}
}
