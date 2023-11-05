class_name RoomTileMapManager extends Node2D

@export var target_tile_map: TileMap
var target_tile_set: TileSet
@export var instruction_tile_map: InstructionTileMap
@export var wfc_generator: WFC2DGenerator
@export var constraint_mapper: TileConstraintMapper
@export var prohibited_tiles: ProhibitedTileCollection
@export var interchangeable_tile_rules_sharer: InterchangeableTileRulesSharer

var _started: bool
var _finished: bool

signal tiles_finished()

func _ready():
	wfc_generator.done.connect(_on_wfc_done)
	# Make sure all child components have reference to the TileMaps
	set_target(target_tile_map)
	set_instruction_tile_map(instruction_tile_map)

func set_target(p_target_tile_map: TileMap):
	target_tile_map = p_target_tile_map
	wfc_generator.target_node = target_tile_map
	if target_tile_map != null:
		_set_target_tile_set(target_tile_map.tile_set)
	else:
		_set_target_tile_set(null)

func _set_target_tile_set(p_target_tile_set: TileSet):
	target_tile_set = p_target_tile_set

func set_instruction_tile_map(p_instruction_tile_map: InstructionTileMap):
	instruction_tile_map = p_instruction_tile_map
	constraint_mapper.instruction_tile_map = instruction_tile_map


func collapse():
	assert(!_started && !_finished)
	_started = true
	wfc_generator.start()
	
func add_positive_sample(positive_sample: TileMap):
	assert(!_started && !_finished)
	wfc_generator.positive_tile_map_set.append(positive_sample)

func add_negative_sample(negative_sample: TileMap):
	assert(!_started && !_finished)
	wfc_generator.negative_tile_map_set.append(negative_sample)

func set_rect(rect: Rect2i):
	wfc_generator.rect = rect
	
func _on_wfc_done():
	assert(_started && !_finished)
	_finished = true
	tiles_finished.emit()
