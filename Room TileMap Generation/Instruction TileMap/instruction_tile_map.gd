class_name InstructionTileMap extends TileMap

@export var instruction_dictionary: TileDictionary

func _ready():
	instruction_dictionary.initialize()

func draw(tile_name: String, coords: Vector2i):
	var tile: TileAttributes = instruction_dictionary.get_tile(tile_name)
	assert(tile != null)
	set_cell(0, coords, tile.source_id, tile.atlas_coords, tile.alternative_tile)
	return
