using Godot;
using System;

namespace ProgJam2023.TileHelper;

public partial class Tile : Resource
{
	
	[Export]
	public TileAttributes Attributes;
	public TileData Data;
	
	public Tile(TileAttributes attributes = null, TileData data = null)
	{
		Attributes = attributes;
		Data = data;
	}
	
	public static Tile FromTileMap(TileMap tileMap, int layer, Vector2I coords, bool useProxies = false)
	{
		if(tileMap.GetCellSourceId(layer, coords, useProxies) == -1)
		{
			return null;
		}
		TileAttributes attributes = TileAttributes.FromTileMap(tileMap, layer, coords, useProxies);
		TileData data = tileMap.GetCellTileData(layer, coords, useProxies);
		return new Tile(attributes, data);
	}
	
}
