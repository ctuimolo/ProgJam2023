class_name TileConstraintMapperConstraint extends Resource

@export var constraint_tag: String
@export var domain_tag: String

var constraint_set: TileTagSet = null
var domain_set: TileTagSet = null

var tile_constraint_mapper: TileConstraintMapper
var instruction_tile_set: TileSet
var target_tile_set: TileSet
var custom_data_layer: String
var _initialized: bool = false

var _domain_bitset: WFCBitSet = null

func _init(p_constraint_tag: String = "", p_domain_tag: String = ""):
	constraint_tag = p_constraint_tag
	domain_tag = p_domain_tag

# Called by TileConstraintMapper to initialize variables
func initialize(p_tile_constraint_mapper: TileConstraintMapper, p_instruction_tile_set: TileSet, p_target_tile_set: TileSet, p_custom_data_layer: String):
	if _initialized:
		return
	_initialized = true
	assert(p_instruction_tile_set != null)
	assert(p_target_tile_set != null)
	tile_constraint_mapper = p_tile_constraint_mapper
	instruction_tile_set = p_instruction_tile_set
	target_tile_set = p_target_tile_set
	custom_data_layer = p_custom_data_layer
	constraint_set = TileTagSet.new(instruction_tile_set, custom_data_layer, [constraint_tag])
	domain_set = TileTagSet.new(target_tile_set, custom_data_layer, [domain_tag])

# Get a bitset containing the tiles from the constraint's domain
func get_domain_bitset()->WFCBitSet:
	assert(_initialized)
	# Return cached bitset if already created
	if _domain_bitset != null:
		return _domain_bitset
	# Create a new bitset and set the bits of all tiles in the domain set
	_domain_bitset = tile_constraint_mapper.get_empty_bitset()
	for tile in domain_set.get_tiles():
		var tile_id = tile_constraint_mapper.get_tile_id(tile.attributes)
		if tile_id > -1:
			_domain_bitset.set_bit(tile_id)
	return _domain_bitset
