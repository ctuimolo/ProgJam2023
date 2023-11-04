class_name TestRoomGenerator extends Node2D

@export var rect: Rect2i
var min: Vector2i:
	get: return rect.position
var max: Vector2i:
	get: return rect.end - Vector2i(1, 1)

@export var tile_map_manager: RoomTileMapManager
@export var drawer: RoomTileMapDrawer

var _up_door: Vector2i
var _down_door: Vector2i
var _left_door: Vector2i
var _right_door: Vector2i

var rng: RandomNumberGenerator

# Called when the node enters the scene tree for the first time.
func _ready():
	rng = RandomNumberGenerator.new()
	# Draw instructions
	_draw_outer_walls()
	_initialize_doors()
	_draw_doors()
	_draw_paths_between_doors()
	# Generate room
	tile_map_manager.set_rect(rect)
	tile_map_manager.collapse()

func _draw_outer_walls():
	for x in range(min.x, max.x):
		drawer.draw_instruction("wall", Vector2i(x, min.y))
		drawer.draw_instruction("wall", Vector2i(x, max.y))
	for y in range(min.y, max.y):
		drawer.draw_instruction("wall", Vector2i(min.x, y))
		drawer.draw_instruction("wall", Vector2i(max.x, y))
	drawer.draw_instruction("wall", Vector2i(max.x, max.y))

func _draw_doors():
	drawer.draw_instruction("door", _up_door)
	drawer.draw_instruction("door", _down_door)
	drawer.draw_instruction("door", _left_door)
	drawer.draw_instruction("door", _right_door)

func _draw_paths_between_doors():
	_draw_path(_up_door + Vector2i(0, 1), _down_door + Vector2i(0, -1))
	_draw_path(_left_door + Vector2i(1, 0), _right_door + Vector2i(-1, 0))

func _draw_path(a: Vector2i, b: Vector2i):
	var path = _get_path_between(a, b)
	for coords in path:
		drawer.draw_instruction("floor", coords)

func _get_path_between(a: Vector2i, b: Vector2i)->Array:
	var difference = b - a
	var step_x = Vector2i(1 if difference.x > 0 else -1, 0)
	var step_y = Vector2i(0, 1 if difference.y > 0 else -1)
	var coords = a
	var result: Array[Vector2i] = []
	while coords != b:
		result.append(coords)
		if coords.x == b.x:
			coords += step_y
		elif coords.y == b.y:
			coords += step_x
		elif randi_range(0, 1) == 0:
			coords += step_y
		else:
			coords += step_x
	result.append(b)
	return result

func _initialize_doors():
	_up_door = Vector2i(rng.randi_range(min.x + 1, max.x - 1), min.y)
	_down_door = Vector2i(rng.randi_range(min.x + 1, max.x - 1), max.y)
	_left_door = Vector2i(min.x, rng.randi_range(min.y + 1, max.y - 1))
	_right_door = Vector2i(max.x, rng.randi_range(min.y + 1, max.y - 1))

