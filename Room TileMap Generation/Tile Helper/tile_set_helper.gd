class_name TileSetHelper extends RefCounted

func get_all_tiles_in_set(tile_set: TileSet)->Array:
	var result: Array[Tile] = []
	# Each source in the TileSet
	for source in tile_set.get_source_count():
		var source_id: int = tile_set.get_source_id(source)
		var tiles: Array[Tile] = get_all_tiles_in_source(tile_set, source_id)
		result.append_array(tiles)
	return result

func get_all_tiles_in_source(tile_set: TileSet, source_id: int)->Array:
	var result: Array[Tile] = []
	# Only consider if TileSetAtlasSource
	if !(tile_set.get_source(source_id) is TileSetAtlasSource):
		return result
	var source: TileSetAtlasSource = tile_set.get_source(source_id)
	# Iterate through tiles
	for tile_index in source.get_tiles_count():
		var coords: Vector2i = source.get_tile_id(tile_index)
		var alternative_count: int = source.get_alternative_tiles_count(coords)
		# Iterate through alternative tiles
		for alternativeIndex in range(alternative_count):
			var alternative: int = source.get_alternative_tile_id(coords, alternativeIndex)
			var attributes: TileAttributes = TileAttributes.new(source_id, coords, alternative)
			var tile_data: TileData = source.get_tile_data(coords, alternative)
			var tile: Tile = Tile.new(attributes, tile_data)
			result.append(tile)
	return result
