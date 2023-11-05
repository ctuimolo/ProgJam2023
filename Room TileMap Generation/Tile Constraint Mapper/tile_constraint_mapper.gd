class_name TileConstraintMapper extends Node

@export var custom_data_layer: String = "tags"
@export var instruction_tile_map: TileMap
@export var constraints: Array[TileConstraintMapperConstraint]

var problem: WFC2DProblem
var tile_set: TileSet:
	get:
		return problem.rules.mapper.tile_set

var _initialized: bool = false
func initialize():
	assert(!_initialized)
	_initialized = true
	for constraint in constraints:
		constraint.initialize(self, instruction_tile_map.tile_set, tile_set, custom_data_layer)

func read_tile_constraints(coords: Vector2i)->WFCBitSet:
	if !_initialized:
		initialize()
	var tile = Tile.from_tile_map(instruction_tile_map, 0, coords)
	# Initialize the set of cells that can go here, starting as none
	var domain: WFCBitSet = get_empty_bitset()
	# No constraints if tile is empty
	if tile == null:
		domain.set_all()
		return domain
	# Go through each constraint
	var has_constraint: bool = false
	for constraint in constraints:
		# Ignore if this constraint does not apply
		if !constraint.constraint_set.belongs_to_set(tile):
			continue
		#print(coords, " is ", constraint.constraint_set.included_tags[0])
		has_constraint = true
		# Add all the cells that are allowed for this constraint
		var constraint_domain_bitset: WFCBitSet = constraint.get_domain_bitset()
		domain.union_in_place(constraint_domain_bitset)
	# If no constraints applied, do not restrict domain
	if !has_constraint:
		domain.set_all()
	return domain

# Get an empty WFCBitSet with the correct size
func get_empty_bitset()->WFCBitSet:
	return WFCBitSet.new(problem.rules.mapper.size(), false)

# Get the id of a tile.
# The id can be used to add or remove it from a WFCBitSet
func get_tile_id(attributes: TileAttributes)->int:
	var tile_attrs: Vector4i = attributes.to_Vector4i()
	return problem.get_tile_id(tile_attrs)
