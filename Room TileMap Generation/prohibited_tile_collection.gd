class_name ProhibitedTileCollection extends Node

# Custom data layer with boolean value to determine if tile CANNOT be placed
@export var custom_data_layer_name: String
@export var target_tile_set: TileSet

var problem: WFC2DProblem

var _bitset: WFCBitSet = null

func get_bitset()->WFCBitSet:
	if _bitset != null:
		return _bitset
	_bitset = get_full_bitset()
	var prohibited_tiles: Array[Tile] = get_prohibited_tiles()
	for tile in prohibited_tiles:
		var tile_id = get_tile_id(tile.attributes)
		if tile_id > 0:
			_bitset.set_bit(tile_id, false)
	return _bitset

func get_prohibited_tiles()->Array:
	var result: Array[Tile] = []
	var tiles: Array[Tile] = TileSetHelper.new().get_all_tiles_in_set(target_tile_set)
	for tile in tiles:
		var cannot_place: bool = tile.data.get_custom_data(custom_data_layer_name)
		if cannot_place:
			result.append(tile)
	return result

func get_tile_id(attributes: TileAttributes)->int:
	var tile_attrs: Vector4i = attributes.to_Vector4i()
	return problem.get_tile_id(tile_attrs)

func get_full_bitset()->WFCBitSet:
	return WFCBitSet.new(problem.rules.mapper.size(), true)
