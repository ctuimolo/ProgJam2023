using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.TileHelper;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomTileMapEditor : Node2D
{
	
	[Export]
	public TileMap TargetTileMap;
	[Export]
	public InstructionTileMap Instructions;
	
	public ClaimedCellCollection ClaimedCellCollection = new ClaimedCellCollection();
	
	// Remove after path generation is moved somewhere else
	public RandomNumberGenerator RNG;
	
	private void SetCell(Vector2I cell, TileAttributes attributes)
	{
		TargetTileMap.SetCell(0, cell, attributes.SourceID, attributes.AtlasCoords, attributes.AlternativeTile);
	}
	
	// Draw a tile on a cell
	public void DrawTile(Vector2I cell, Tile tile, StringName claimID) => DrawTile(cell, tile.Attributes, claimID);
	public void DrawTile(Vector2I cell, TileAttributes attributes, StringName claimID)
	{
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCell(cell, claimID);
		}
		SetCell(cell, attributes);
	}
	
	// Draw one tile over several cells
	public void DrawTiles(IEnumerable<Vector2I> cells, Tile tile, StringName claimID) => DrawTiles(cells, tile.Attributes, claimID);
	public void DrawTiles(IEnumerable<Vector2I> cells, TileAttributes attributes, StringName claimID)
	{
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCells(cells, claimID);
		}
		foreach(Vector2I cell in cells)
		{
			SetCell(cell, attributes);
		}
	}
	
	// Draw several tiles over several cells
	public void DrawTiles(IEnumerable<Vector2I> cells, IEnumerable<Tile> tiles, StringName claimID)
	{
		List<Vector2I> cellsList = new List<Vector2I>(cells);
		List<TileAttributes> tilesList = new List<TileAttributes>();
		foreach(Tile tile in tiles)
		{
			tilesList.Add(tile.Attributes);
		}
		DrawTiles(cellsList, tilesList, claimID);
	}
	public void DrawTiles(IEnumerable<Vector2I> cells, IEnumerable<TileAttributes> tiles, StringName claimID)
	{
		DrawTiles(new List<Vector2I>(cells), new List<TileAttributes>(tiles), claimID);
	}
	public void DrawTiles(IList<Vector2I> cells, IList<Tile> tiles, StringName claimID)
	{
		List<TileAttributes> tileAttributesList = new List<TileAttributes>();
		foreach(Tile tile in tiles)
		{
			tileAttributesList.Add(tile.Attributes);
		}
		DrawTiles(cells, tileAttributesList, claimID);
	}
	public void DrawTiles(IList<Vector2I> cells, IList<TileAttributes> tiles, StringName claimID)
	{
		if(tiles.Count != cells.Count) throw new ArgumentException();
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCells(cells, claimID);
		}
		for(int i = 0; i < cells.Count; i++)
		{
			SetCell(cells[i], tiles[i]);
		}
	}
	
	public void DrawPattern(Vector2I drawPoint, TileMapPattern pattern, StringName claimID)
	{
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCells(drawPoint, pattern, claimID);
		}
		TargetTileMap.SetPattern(0, drawPoint, pattern);
	}
	
	
	public void DrawInstruction(Vector2I cell, string instruction, StringName claimID)
	{
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCell(cell, claimID);
		}
		Instructions.Draw(instruction, cell);
	}
	public void DrawInstructions(IEnumerable<Vector2I> cells, string instruction, StringName claimID)
	{
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCells(cells, claimID);
		}
		foreach(Vector2I cell in cells)
		{
			Instructions.Draw(instruction, cell);
		}
	}
	public void DrawInstructions(IEnumerable<Vector2I> cells, IEnumerable<string> instructions, StringName claimID)
	{
		DrawInstructions(new List<Vector2I>(cells), new List<string>(instructions), claimID);
	}
	public void DrawInstructions(IList<Vector2I> cells, IList<string> instructions, StringName claimID)
	{
		if(instructions.Count != cells.Count) throw new ArgumentException();
		if(claimID != null)
		{
			ClaimedCellCollection.ClaimCells(cells, claimID);
		}
		for(int i = 0; i < cells.Count; i++)
		{
			Instructions.Draw(instructions[i], cells[i]);
		}
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
	
	public void DrawInstructionRectOutline(Rect2I rect, string instruction, StringName claimID)
	{
		DrawInstructionRectOutline(rect.Position, rect.End, instruction, claimID);
	}
	public void DrawInstructionRectOutline(Vector2I min, Vector2I max, string instruction, StringName claimID)
	{
		List<Vector2I> cells = GetRectOutlineCells(min, max);
		DrawInstructions(cells, instruction, claimID);
	}
	
	public List<Vector2I> GetRectOutlineCells(Rect2I rect)
	{
		return GetRectOutlineCells(rect.Position, rect.End);
	}
	public List<Vector2I> GetRectOutlineCells(Vector2I min, Vector2I max)
	{
		List<Vector2I> cells = new List<Vector2I>();
		for(int x = min.X; x < max.X; x++)
		{
			cells.Add(new Vector2I(x, min.Y));
			cells.Add(new Vector2I(x, max.Y));
		}
		for(int y = min.Y; y < max.Y; y++)
		{
			cells.Add(new Vector2I(min.X, y));
			cells.Add(new Vector2I(max.X, y));
		}
		cells.Add(max);
		return cells;
	}
	
	public void DrawDoor(RoomDesigner.Door door, StringName claimID)
	{
		DrawPattern(door.Position - door.PatternCenter, door.TileMapPattern, claimID);
	}
	
	//////////////////////////////////////////////////////////////
	
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
