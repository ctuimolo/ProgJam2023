using Godot;
using System;

namespace ProgJam2023.TileHelper;

public partial class TileAttributes : Resource
{
	
	[Export]
	public int SourceID = -1;
	
	[Export]
	public Vector2I AtlasCoords = new Vector2I(-1, -1);
	
	[Export]
	public int AlternativeTile = 0;
	
	public TileAttributes()
	{
		SourceID = -1;
		AtlasCoords = Vector2I.Zero;
		AlternativeTile = 0;
	}
	public TileAttributes(int sourceID, Vector2I atlasCoords, int alternativeTile)
	{
		SourceID = sourceID;
		AtlasCoords = atlasCoords;
		AlternativeTile = alternativeTile;
	}
	
	public Vector4I ToVector4I()
	{
		return new Vector4I(SourceID, AtlasCoords.X, AtlasCoords.Y, AlternativeTile);
	}
	
	public static TileAttributes FromTileMap(TileMap tileMap, int layer, Vector2I coords, bool useProxies = false)
	{
		int sourceID = tileMap.GetCellSourceId(layer, coords, useProxies);
		Vector2I atlasCoords = tileMap.GetCellAtlasCoords(layer, coords, useProxies);
		int alternativeTile = tileMap.GetCellAlternativeTile(layer, coords, useProxies);
		return new TileAttributes(sourceID, atlasCoords, alternativeTile);
	}
	
	public override string ToString()
	{
		return $"{{ SourceID: {SourceID}, AtlasCoords: {AtlasCoords}, AlternativeTile: {AlternativeTile} }}";
	}
}
