class_name InstructionTileMap extends TileMap

@export var instruction_dictionary: TileDictionary

func _ready():
	instruction_dictionary.initialize()

func draw(tile_name: String, cell: Vector2i):
	var tile: TileAttributes = instruction_dictionary.get_tile(tile_name)
	assert(tile != null)
	set_cell(0, cell, tile.source_id, tile.atlas_coords, tile.alternative_tile)

func draw_array(tile_name: String, cells: Array[Vector2i]):
	var tile: TileAttributes = instruction_dictionary.get_tile(tile_name)
	assert(tile != null)
	for cell in cells:
		set_cell(0, cell, tile.source_id, tile.atlas_coords, tile.alternative_tile)
