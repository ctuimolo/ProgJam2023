class_name RoomTileMapManager extends Node2D

@export var target_tile_map: TileMap
@export var instruction_tile_map: TileMap
@export var wfc_generator: WFC2DGenerator

var _started: bool
var _finished: bool

signal tiles_finished()

func _ready():
	wfc_generator.done.connect(_on_wfc_done)

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
