class_name Tile extends Resource

@export var attributes: TileAttributes
var data: TileData

func _init(p_attributes: TileAttributes = null, p_data: TileData = null):
	attributes = p_attributes
	data = p_data

static func from_tile_map(tile_map: TileMap, layer: int, coords: Vector2i, use_proxies: bool = false):
	if tile_map.get_cell_source_id(layer, coords, use_proxies) == -1:
		return null
	var tile_attributes: TileAttributes = TileAttributes.from_tile_map(tile_map, layer, coords, use_proxies)
	var tile_data: TileData = tile_map.get_cell_tile_data(layer, coords, use_proxies)
	return Tile.new(tile_attributes, tile_data)
