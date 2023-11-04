class_name RoomTileMapDrawer extends Node

@export var tile_map_manager: RoomTileMapManager
@export var instruction_dictionary: TileDictionary

func _ready():
	instruction_dictionary.initialize()

func draw_instruction(tile_name: String, coords: Vector2i):
	var tile: TileAttributes = instruction_dictionary.get_tile(tile_name)
	assert(tile != null)
	tile_map_manager.instruction_tile_map.set_cell(0, coords, tile.source_id, tile.atlas_coords, tile.alternative_tile)
	return
