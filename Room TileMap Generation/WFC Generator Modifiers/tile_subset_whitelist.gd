class_name TileSubsetWhitelist extends Node

@export var whitelist: Array[StringName]

@export var custom_data_layer_name: String

var tile_set: TileSet:
	get:
		return problem.rules.mapper.tile_set

var problem: WFC2DProblem

var _bitset: WFCBitSet = null

func get_bitset()->WFCBitSet:
	if _bitset != null:
		return _bitset
	_bitset = get_empty_bitset()
	var whitelisted_tiles: Array[Tile] = get_whitelisted_tiles()
	for tile in whitelisted_tiles:
		var tile_id = get_tile_id(tile.attributes)
		if tile_id > 0:
			_bitset.set_bit(tile_id, true)
	return _bitset

func get_whitelisted_tiles()->Array:
	var result: Array[Tile] = []
	var tiles: Array[Tile] = TileSetHelper.new().get_all_tiles_in_set(tile_set)
	for tile in tiles:
		var tile_is_whitelisted: bool = true
		for subset in get_tile_subsets(tile):
			if !whitelist.has(subset):
				tile_is_whitelisted = false
				break
		if tile_is_whitelisted:
			result.append(tile)
	return result

func get_tile_subsets(tile: Tile)->Array:
	var subsets_string: String = tile.data.get_custom_data(custom_data_layer_name)
	if subsets_string == null || len(subsets_string) == 0:
		return []
	return subsets_string.split(",")

func get_tile_id(attributes: TileAttributes)->int:
	var tile_attrs: Vector4i = attributes.to_Vector4i()
	return problem.get_tile_id(tile_attrs)

func get_empty_bitset()->WFCBitSet:
	return WFCBitSet.new(problem.rules.mapper.size(), false)
