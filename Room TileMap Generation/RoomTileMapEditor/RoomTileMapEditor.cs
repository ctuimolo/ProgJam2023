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
	public void DrawTile(Tile tile, Vector2I cell, bool claim) => DrawTile(tile.Attributes, cell, claim);
	public void DrawTile(TileAttributes attributes, Vector2I cell, bool claim)
	{
		if(ClaimedCellCollection.CellIsClaimed(cell)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCell(cell);
		}
		SetCell(cell, attributes);
	}
	
	// Draw one tile over several cells
	public void DrawTiles(Tile tile, IEnumerable<Vector2I> cells, bool claim) => DrawTiles(tile.Attributes, cells, claim);
	public void DrawTiles(TileAttributes attributes, IEnumerable<Vector2I> cells, bool claim)
	{
		if(ClaimedCellCollection.ContainsClaimedCells(cells)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCells(cells);
		}
		foreach(Vector2I cell in cells)
		{
			SetCell(cell, attributes);
		}
	}
	
	// Draw several tiles over several cells
	public void DrawTiles(IEnumerable<Tile> tiles, IEnumerable<Vector2I> cells, bool claim)
	{
		List<TileAttributes> tilesList = new List<TileAttributes>();
		List<Vector2I> cellsList = new List<Vector2I>();
		foreach(Tile tile in tiles)
		{
			tilesList.Add(tile.Attributes);
		}
		foreach(Vector2I cell in cells)
		{
			cellsList.Add(cell);
		}
		DrawTiles(tilesList, cellsList, claim);
	}
	public void DrawTiles(IEnumerable<TileAttributes> tiles, IEnumerable<Vector2I> cells, bool claim)
	{
		List<TileAttributes> tilesList = new List<TileAttributes>();
		List<Vector2I> cellsList = new List<Vector2I>();
		foreach(TileAttributes tileAttributes in tiles)
		{
			tilesList.Add(tileAttributes);
		}
		foreach(Vector2I cell in cells)
		{
			cellsList.Add(cell);
		}
		DrawTiles(tilesList, cellsList, claim);
	}
	public void DrawTiles(IList<Tile> tiles, IList<Vector2I> cells, bool claim)
	{
		List<TileAttributes> tileAttributesList = new List<TileAttributes>();
		foreach(Tile tile in tiles)
		{
			tileAttributesList.Add(tile.Attributes);
		}
		DrawTiles(tileAttributesList, cells, claim);
	}
	public void DrawTiles(IList<TileAttributes> tiles, IList<Vector2I> cells, bool claim)
	{
		if(tiles.Count != cells.Count) throw new ArgumentException();
		if(ClaimedCellCollection.ContainsClaimedCells(cells)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCells(cells);
		}
		for(int i = 0; i < cells.Count; i++)
		{
			SetCell(cells[i], tiles[i]);
		}
	}
	
	public void DrawPattern(TileMapPattern pattern, Vector2I drawPoint, bool claim)
	{
		if(ClaimedCellCollection.PatternAreaContainsClaimedCells(drawPoint, pattern)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimPatternArea(drawPoint, pattern);
		}
		TargetTileMap.SetPattern(0, drawPoint, pattern);
	}
	
	
	public void DrawInstruction(string instruction, Vector2I cell, bool claim)
	{
		if(ClaimedCellCollection.CellIsClaimed(cell)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCell(cell);
		}
		Instructions.Draw(instruction, cell);
	}
	public void DrawInstructions(string instruction, IEnumerable<Vector2I> cells, bool claim)
	{
		if(ClaimedCellCollection.ContainsClaimedCells(cells)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCells(cells);
		}
		foreach(Vector2I cell in cells)
		{
			Instructions.Draw(instruction, cell);
		}
	}
	public void DrawInstructions(IEnumerable<string> instructions, IEnumerable<Vector2I> cells, bool claim)
	{
		List<string> instructionsList = new List<string>();
		List<Vector2I> cellsList = new List<Vector2I>();
		foreach(string instruction in instructions)
		{
			instructionsList.Add(instruction);
		}
		foreach(Vector2I cell in cells)
		{
			cellsList.Add(cell);
		}
		DrawInstructions(instructionsList, cellsList, claim);
	}
	public void DrawInstructions(IList<string> instructions, IList<Vector2I> cells, bool claim)
	{
		if(instructions.Count != cells.Count) throw new ArgumentException();
		if(ClaimedCellCollection.ContainsClaimedCells(cells)) throw new ArgumentException();
		if(claim)
		{
			ClaimedCellCollection.ClaimCells(cells);
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
	
	public void DrawInstructionRectOutline(string instruction, Rect2I rect, bool claim)
	{
		DrawInstructionRectOutline(instruction, rect.Position, rect.End, claim);
	}
	public void DrawInstructionRectOutline(string instruction, Vector2I min, Vector2I max, bool claim)
	{
		List<Vector2I> cells = GetRectOutlineCells(min, max);
		DrawInstructions(instruction, cells, claim);
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
	
	//////////////////////////////////////////////////////////////
	
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
