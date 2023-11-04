class_name TileAttributes extends Resource

@export var source_id: int = -1
@export var atlas_coords: Vector2i = Vector2i(-1, -1)
@export var alternative_tile: int = 0

func _init(p_source_id: int = -1, p_atlas_coords: Vector2i = Vector2i.ZERO, p_alternative_tile: int = -1):
	source_id = p_source_id
	atlas_coords = p_atlas_coords
	alternative_tile = p_alternative_tile

func to_Vector4i()->Vector4i:
	return Vector4i(source_id, atlas_coords.x, atlas_coords.y, alternative_tile)

static func from_tile_map(tile_map: TileMap, layer: int, coords: Vector2i, use_proxies: bool = false):
	var cell_source_id: int = tile_map.get_cell_source_id(layer, coords, use_proxies)
	var cell_atlas_coords: Vector2i = tile_map.get_cell_atlas_coords(layer, coords, use_proxies)
	var cell_alternative_tile: int = tile_map.get_cell_alternative_tile(layer, coords, use_proxies)
	return TileAttributes.new(cell_source_id, cell_atlas_coords, cell_alternative_tile)

func as_string()->String:
	return "source_id: %s, atlas_coords: %s, alternative_tile: %s" % [source_id, atlas_coords, alternative_tile]
