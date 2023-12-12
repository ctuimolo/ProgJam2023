using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.TileHelper;

public static class TileSetHelper
{
	
	public static List<Tile> GetAllTilesInSet(TileSet tileSet)
	{
		List<Tile> result = new List<Tile>();
		for(int i = 0; i < tileSet.GetSourceCount(); i++)
		{
			int sourceID = tileSet.GetSourceId(i);
			List<Tile> tiles = GetAllTilesInSource(tileSet, sourceID);
			result.AddRange(tiles);
		}
		return result;
	}
	
	public static List<Tile> GetAllTilesInSource(TileSet tileSet, int sourceID)
	{
		List<Tile> result = new List<Tile>();
		// Only consider if TileSetAtlasSource
		TileSetAtlasSource source = tileSet.GetSource(sourceID) as TileSetAtlasSource;
		if(source == null)
		{
			return result;
		}
		
		// Iterate through tiles
		for(int tileIndex = 0; tileIndex < source.GetTilesCount(); tileIndex++)
		{
			Vector2I coords = source.GetTileId(tileIndex);
			int alternativeCount = source.GetAlternativeTilesCount(coords);
			// Iterate through alternative tiles
			for(int alternativeIndex = 0; alternativeIndex < alternativeCount; alternativeIndex++)
			{
				int alternative = source.GetAlternativeTileId(coords, alternativeIndex);
				TileAttributes attributes = new TileAttributes(sourceID, coords, alternative);
				TileData tileData = source.GetTileData(coords, alternative);
				Tile tile = new Tile(attributes, tileData);
				result.Add(tile);
			}
		}
		return result;
	}
	
}
