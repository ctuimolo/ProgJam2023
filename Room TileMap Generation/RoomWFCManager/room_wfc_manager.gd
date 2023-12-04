class_name RoomWFCManager extends Node2D

@export var target_tile_map: TileMap
var target_tile_set: TileSet
@export var instruction_tile_map: InstructionTileMap
@export var wfc_generator: WFC2DGenerator
@export var constraint_mapper: TileConstraintMapper
@export var prohibited_tiles: ProhibitedTileCollection
@export var interchangeable_tile_rules_sharer: InterchangeableTileRulesSharer

var _started: bool
var _finished: bool

var _rect: Rect2i
const _MIN_RECT_SIZE: Vector2i = Vector2i(21, 12)

signal tiles_finished()
signal generation_error()

func _ready():
	wfc_generator.done.connect(_on_wfc_done)
	# Make sure all child components have reference to the TileMaps
	set_target_tile_map(target_tile_map)
	set_instruction_tile_map(instruction_tile_map)

func set_target_tile_map(p_target_tile_map: TileMap):
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
	_prepare_wfc_rect()
	wfc_generator.start()

func _prepare_wfc_rect():
	var wfc_rect_size = Vector2i(max(_MIN_RECT_SIZE.x, _rect.size.x), max(_MIN_RECT_SIZE.y, _rect.size.y))
	wfc_generator.rect = Rect2i(_rect.position, wfc_rect_size)
	_fill_area_outside_rect()

func _fill_area_outside_rect():
	for x in range(_rect.size.x, _MIN_RECT_SIZE.x):
		for y in range(_rect.size.y):
			var cell = Vector2i(x, y) + _rect.position
			instruction_tile_map.draw("black", cell)
	for y in range(_rect.size.y, _MIN_RECT_SIZE.y):
		for x in range(_rect.size.x):
			var cell = Vector2i(x, y) + _rect.position
			instruction_tile_map.draw("black", cell)
	for x in range(_rect.size.x, _MIN_RECT_SIZE.x):
		for y in range(_rect.size.y, _MIN_RECT_SIZE.y):
			var cell = Vector2i(x, y) + _rect.position
			instruction_tile_map.draw("black", cell)
	
func add_positive_sample(positive_sample: TileMap):
	assert(!_started && !_finished)
	wfc_generator.positive_tile_map_set.append(positive_sample)

func add_negative_sample(negative_sample: TileMap):
	assert(!_started && !_finished)
	wfc_generator.negative_tile_map_set.append(negative_sample)

func set_rect(p_rect: Rect2i):
	_rect = p_rect
	
func _on_wfc_done(error: bool):
	assert(_started && !_finished)
	_finished = true
	if error:
		generation_error.emit()
	else:
		tiles_finished.emit()
