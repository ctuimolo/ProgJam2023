class_name InterchangeableTileRulesSharer extends Node

@export var custom_data_layer_name: String = "interchangeable"

var wfc_rules: WFCRules2D
var tile_set: TileSet:
	get:
		return wfc_rules.mapper.tile_set

func share_interchangeable_tile_rules(positive: bool):
	var tag_to_tiles = _get_interchangeable_dictionary(tile_set)
	for tiles in tag_to_tiles.values():
		share_tile_rules(tiles, positive)

# Set the rules of each tile in the array to be a combination of rules from all the tiles
func share_tile_rules(tiles: Array, positive: bool):
	#print("Initial:")
	#_print_all_rules(tiles)
	# Combine the rules for each of the tiles
	var combined_rules: Array[WFCBitSet] = get_combined_rules(tiles, positive)
	for tile in tiles:
		set_rules(tile, copy_rules(combined_rules))
	#print("Copied rows:")
	#_print_all_rules(tiles)
	# Make every other tile treat them all interchangeably
	for matrix in wfc_rules.axis_matrices:
		for row in matrix.rows:
			make_interchangeable_in_row(tiles, row, positive)
	#print("Final")
	#_print_all_rules(tiles)

func _print_all_rules(tiles: Array):
	for tile in tiles:
		_print_rules(tile)

func _print_rules(tile: Tile):
	var rules: Array[WFCBitSet] = get_rules(tile)
	var output: String = "["
	for i in range(len(rules)):
		if i > 0:
			output += ", "
		output += rules[i].format_bits()
	output += "]"
	print(output)
	
# Share value between tiles in a WFCBitSet
# if positive is true, every tile will have its bit set if one of them does
# if positive is false, every tile will have its bit not set if one of them is not set
func make_interchangeable_in_row(tiles: Array, row: WFCBitSet, positive: bool):
	var shared_value: bool = !positive
	for tile in tiles:
		var id: int = get_tile_id(tile.attributes)
		var tile_value = row.get_bit(id)
		if positive == tile_value:
			shared_value = tile_value
			break
	for tile in tiles:
		var id: int = get_tile_id(tile.attributes)
		row.set_bit(id, shared_value)

# Combine the rules of all the tiles using union or intersection
func get_combined_rules(tiles: Array, positive: bool)->Array:
	assert(len(tiles) > 0)
	# Get Array of each tile's rules
	var rules_sets: = []
	for tile in tiles:
		rules_sets.append(get_rules(tile))
	# Combine rules from all tiles
	var combined_rules: Array[WFCBitSet] = copy_rules(rules_sets[0])
	for rules in rules_sets:
		if positive:
			union_rules(combined_rules, rules)
		else:
			intersect_rules(combined_rules, rules)
	return combined_rules

func union_rules(rules: Array[WFCBitSet], other: Array[WFCBitSet]):
	assert(len(rules) == len(other))
	for i in range(len(rules)):
		rules[i].union_in_place(other[i])

func intersect_rules(rules: Array[WFCBitSet], other: Array[WFCBitSet]):
	assert(len(rules) == len(other))
	for i in range(len(rules)):
		rules[i].intersect_in_place(other[i])

func copy_rules(rules: Array[WFCBitSet])->Array:
	var result: Array[WFCBitSet] = []
	for bit_set in rules:
		result.append(bit_set.copy())
	return result

# Returns Array[WFCBitSet]
# Each element corresponds to the rules for the given tile for each axis
# Returned WFCBitSet are reference, not copy
func get_rules(tile: Tile)->Array:
	var id: int = get_tile_id(tile.attributes)
	var result: Array[WFCBitSet] = []
	for matrix in wfc_rules.axis_matrices:
		result.append(matrix.rows[id])
	return result

func set_rules(tile: Tile, rules: Array[WFCBitSet]):
	var id: int = get_tile_id(tile.attributes)
	assert(len(rules) == len(wfc_rules.axis_matrices))
	for i in range(len(rules)):
		var rule: WFCBitSet = rules[i]
		var matrix: WFCBitMatrix = wfc_rules.axis_matrices[i]
		matrix.rows[id] = rule

# Get dictionary mapping interchangeable tag to all tiles with that tag
func _get_interchangeable_dictionary(tile_set: TileSet)->Dictionary:
	var tag_to_tiles: Dictionary = { }
	var tiles: Array[Tile] = TileSetHelper.new().get_all_tiles_in_set(tile_set)
	for tile in tiles:
		var tag = tile.data.get_custom_data(custom_data_layer_name)
		if tag == null || len(tag) == 0:
			continue
		if !tag_to_tiles.has(tag):
			tag_to_tiles[tag] = []
		tag_to_tiles[tag].append(tile)
	return tag_to_tiles

func get_tile_id(attributes: TileAttributes)->int:
	if !wfc_rules.mapper.attrs_to_id.has(attributes.to_Vector4i()):
		return -1
	return wfc_rules.mapper.attrs_to_id[attributes.to_Vector4i()]
