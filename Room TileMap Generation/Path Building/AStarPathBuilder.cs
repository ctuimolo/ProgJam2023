using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration.Paths;

public partial class AStarPathBuilder : RefCounted
{
	public AStar2D AStar;
	public AStarGraphBuilder GraphBuilder;
	public RoomTileMapEditor TileMapEditor;
	public PathCombiner PathCombiner;
	
	public AStarPathBuilder(AStar2D aStar, AStarGraphBuilder graphBuilder, RoomTileMapEditor tileMapEditor, PathCombiner pathCombiner)
	{
		AStar = aStar;
		GraphBuilder = graphBuilder;
		TileMapEditor = tileMapEditor;
		PathCombiner = pathCombiner;
	}
	
	public CellPath GetPath(Vector2I a, Vector2I b, PathPointTemplate template, PathCombineMode mode)
	{
		CellEvaluator evaluator = new CellEvaluator(TileMapEditor, template, mode, PathCombiner);
		GraphBuilder.BuildGraph(evaluator);
		int aID = GraphBuilder.GetID(a);
		int bID = GraphBuilder.GetID(b);
		Vector2[] points = AStar.GetPointPath(aID, bID);
		List<Vector2I> pointsList = new List<Vector2I>();
		foreach(Vector2 point in points)
		{
			pointsList.Add((Vector2I)point);
		}
		CellPath path = new CellPath(pointsList, template);
		PathCombiner.RemoveNonOverwriteCells(path, mode, TileMapEditor);
		return path;
	}
	
	private class CellEvaluator : ICellEvaluator
	{
		public RoomTileMapEditor TileMapEditor;
		public PathPointTemplate Template;
		public PathCombineMode Mode;
		
		private PathCombiner PathCombiner;
		
		public CellEvaluator(RoomTileMapEditor tileMapEditor, PathPointTemplate template, PathCombineMode mode, PathCombiner pathCombiner)
		{
			TileMapEditor = tileMapEditor;
			Template = template;
			Mode = mode;
			PathCombiner = pathCombiner;
		}
		
		private StringName GetClaimID(Vector2I cell) => TileMapEditor.ClaimedCellCollection.GetClaimID(cell);
		
		public float GetWeight(Vector2I cell)
		{
			if(!PathCanOverlap(cell))
			{
				return float.PositiveInfinity;
			}
			return 1f;
		}
		
		// Check this cell and surrounding cells to determine if the path can be drawn here
		private bool PathCanOverlap(Vector2I cell)
		{
			foreach(Vector2I offset in Template.Floors)
			{
				StringName claimID = GetClaimID(cell + offset);
				if(!PathCombiner.FloorCanOverlap(claimID, Mode))
				{
					return false;
				}
			}
			foreach(Vector2I offset in Template.Walls)
			{
				StringName claimID = GetClaimID(cell + offset);
				if(!PathCombiner.WallCanOverlap(claimID, Mode))
				{
					return false;
				}
			}
			return true;
		}
		
		public IList<Vector2I> GetNeighbors(Vector2I cell)
		{
			return new Vector2I[] {
				cell + new Vector2I(-1, 0),
				cell + new Vector2I(1, 0),
				cell + new Vector2I(0, -1),
				cell + new Vector2I(0, 1)
			};
		}
	}
}
