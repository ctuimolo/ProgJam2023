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
		return GetPath(a, false, b, false, template, mode);
	}
	public CellPath GetPath(Vector2I a, bool aOpen, Vector2I b, bool bOpen, PathPointTemplate template, PathCombineMode mode)
	{
		CellEvaluator evaluator = new CellEvaluator(this, template, mode);
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
		if(points.Length > 2)
		{
			Vector2I min = PathShapeHelper.FindMin(template.Walls);
			Vector2I max = PathShapeHelper.FindMax(template.Walls);
			if(aOpen)
			{
				Vector2I a1 = new Vector2I((int)points[1].X, (int)points[1].Y);
				OpenPathEnd(path, a, a1 - a, template);
			}
			if(bOpen)
			{
				Vector2I b1 = new Vector2I((int)points[points.Length - 2].X, (int)points[points.Length - 2].Y);
				OpenPathEnd(path, b, b1 - b, template);
			}
		}
		PathCombiner.RemoveNonOverwriteCells(path, mode, TileMapEditor);
		return path;
	}
	
	// Remove walls for an end point of a path
	private void OpenPathEnd(CellPath path, Vector2I end, Vector2I direction, PathPointTemplate template)
	{
		Vector2 center = PathShapeHelper.FindCenter(template.Walls);
		
		Vector2I min = new Vector2I(int.MinValue, int.MinValue);
		Vector2I max = new Vector2I(int.MaxValue, int.MaxValue);
		if(direction == new Vector2I(0, 1))
		{
			min.Y = (int)Math.Floor(center.Y);
		}
		else if(direction == new Vector2I(0, -1))
		{
			max.Y = (int)Math.Ceiling(center.Y);
		}
		else if(direction == new Vector2I(1, 0))
		{
			min.X = (int)Math.Floor(center.X);
		}
		else if(direction == new Vector2I(-1, 0))
		{
			max.X = (int)Math.Ceiling(center.X);
		}
		else
		{
			throw new ArgumentException();
		}
		
		foreach(Vector2I cell in template.Walls)
		{
			if(cell.X < min.X || cell.X > max.X || cell.Y < min.Y || cell.Y > max.Y)
			{
				path.Walls.Remove(cell + end);
			}
		}
	}
	
	private class CellEvaluator : ICellEvaluator
	{
		public AStarPathBuilder PathBuilder;
		public PathPointTemplate Template;
		public PathCombineMode Mode;
		
		public RoomTileMapEditor TileMapEditor => PathBuilder.TileMapEditor;
		private PathCombiner PathCombiner => PathBuilder.PathCombiner;
		
		
		public CellEvaluator(AStarPathBuilder pathBuilder, PathPointTemplate template, PathCombineMode mode)
		{
			PathBuilder = pathBuilder;
			Template = template;
			Mode = mode;
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
