class_name TileTagSet extends Resource

@export var tile_set: TileSet

@export var custom_data_layer: String
@export var included_tags: Array[String]

func _init(p_tile_set: TileSet = null, p_custom_data_layer: String = "", p_included_tags: Array[String] = []):
	tile_set = p_tile_set
	custom_data_layer = p_custom_data_layer
	included_tags = p_included_tags

func get_tiles()->Array:
	var all_tiles: Array[Tile] = TileSetHelper.new().get_all_tiles_in_set(tile_set)
	var tagged: Array[Tile] = []
	for tile in all_tiles:
		var tag = tile.data.get_custom_data(custom_data_layer)
		if included_tags.has(tag):
			tagged.append(tile)
	return tagged

func belongs_to_set(tile: Tile)->bool:
	var tag = tile.data.get_custom_data(custom_data_layer)
	if tag == null:
		return false
	return included_tags.has(tag as String)
