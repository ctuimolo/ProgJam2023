using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.RoomTileMapGeneration.Paths;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomPathDrawer : RefCounted
{	
	public RoomTileMapEditor TileMapEditor;
	private Rect2I Rect;
	
	private AStar2D AStar;
	public HashSet<Vector2I> Cells;
	
	private PathCombiner PathCombiner;
	private AStarGraphBuilder GraphBuilder;
	private AStarPathBuilder PathBuilder;
	
	StringName FloorID, WallID;
	
	public RoomPathDrawer(RoomTileMapEditor tileMapEditor, Rect2I rect, StringName floorClaimID, StringName wallClaimID)
	{
		TileMapEditor = tileMapEditor;
		Rect = rect;
		
		FloorID = floorClaimID;
		WallID = wallClaimID;
		
		AStar = new AStar2D();
		Cells = new HashSet<Vector2I>();
		for(int x = Rect.Position.X; x < Rect.End.X; x++)
		{
			for(int y = Rect.Position.Y; y < Rect.End.Y; y++)
			{
				Cells.Add(new Vector2I(x, y));
			}
		}
		
		PathCombiner = new PathCombiner(FloorID, WallID);
		PathCombiner.FloorOverlaps = new HashSet<StringName>() {
			"StartingCell",
			"Door"
		};
		PathCombiner.WallOverlaps = new HashSet<StringName>() {
			"RoomOutline"
		};
		
		GraphBuilder = new AStarGraphBuilder(TileMapEditor.TargetTileMap, Cells, AStar);
		PathBuilder = new AStarPathBuilder(AStar, GraphBuilder, TileMapEditor, PathCombiner);
	}
	
	public void DrawPath(Vector2I a, Vector2I b, PathPointTemplate template, PathCombineMode mode)
	{
		CellPath path = PathBuilder.GetPath(a, b, template, mode);
		if(path.Points.Count == 0 && a != b)
		{
			GD.Print($"Path error: {a} -> {b}");
		}
		else
		{
			//path.PrintPoints();
		}
		TileMapEditor.DrawInstructions(path.Floors, "floor", FloorID);
		TileMapEditor.DrawInstructions(path.Walls, "wall", WallID);
	}
	
	public void DrawPath(Vector2I a, Vector2I b, int thickness, bool walled, PathCombineMode mode)
	{
		PathPointTemplate template = new PathPointTemplate(thickness, walled);
		DrawPath(a, b, template, mode);
	}
	
}
