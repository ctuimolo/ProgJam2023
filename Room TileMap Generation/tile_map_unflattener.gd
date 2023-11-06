class_name TileMapUnflattener extends Node

@export var custom_data_layer_name: String = "target_layer"
@export var flat_tile_map: TileMap
@export var final_tile_map: TileMap

func unflatten():
	for coords in flat_tile_map.get_used_cells(0):
		var tile: Tile = Tile.from_tile_map(flat_tile_map, 0, coords)
		var target_layer = get_target_layer(tile)
		final_tile_map.set_cell(target_layer, coords, tile.attributes.source_id, tile.attributes.atlas_coords, tile.attributes.alternative_tile)

func get_target_layer(tile: Tile)->int:
	return tile.data.get_custom_data(custom_data_layer_name)
