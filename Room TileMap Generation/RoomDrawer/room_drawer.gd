class_name RoomDrawer extends Node2D

@export var rect: Rect2i
var bounds_min: Vector2i:
	get: return rect.position
var bounds_max: Vector2i:
	get: return rect.end - Vector2i(1, 1)

@export var target_tile_map: TileMap
@export var instructions: InstructionTileMap

@export var north_door_pattern: DoorPattern
@export var south_door_pattern: DoorPattern
@export var east_door_pattern: DoorPattern
@export var west_door_pattern: DoorPattern

var north_door: Door
var south_door: Door
var east_door: Door
var west_door: Door

func north_door_position()->Vector2i:
	return north_door.position
func south_door_position()->Vector2i:
	return south_door.position
func east_door_position()->Vector2i:
	return east_door.position
func west_door_position()->Vector2i:
	return west_door.position

var starting_cell: Vector2i

var rng: RandomNumberGenerator

class Door:
	var pattern: DoorPattern
	var position: Vector2i
	var enter_direction: Vector2i:
		get:
			return pattern.enter_direction
	var enter_tile: Vector2i:
		get:
			return position - pattern.enter_direction
	func _init(p_pattern: DoorPattern, p_position: Vector2i = Vector2i(0, 0)):
		pattern = p_pattern
		position = p_position

# Called when the node enters the scene tree for the first time.
func _ready():
	rng = RandomNumberGenerator.new()

func draw_room():
	# Draw instructions and doors
	_draw_outer_walls()
	_initialize_doors()
	_draw_doors()
	_initialize_starting_cell()
	_draw_door_paths()

func _draw_outer_walls():
	for x in range(bounds_min.x, bounds_max.x):
		instructions.draw("black", Vector2i(x, bounds_min.y))
		instructions.draw("black", Vector2i(x, bounds_max.y))
	for y in range(bounds_min.y, bounds_max.y):
		instructions.draw("black", Vector2i(bounds_min.x, y))
		instructions.draw("black", Vector2i(bounds_max.x, y))
	instructions.draw("black", Vector2i(bounds_max.x, bounds_max.y))

# Draw door tiles on target TileMap
func _draw_door(door: Door):
	target_tile_map.set_pattern(0, door.position - door.pattern.center, door.pattern.tile_map_pattern)

func _draw_doors():
	_draw_door(north_door)
	_draw_door(south_door)
	_draw_door(east_door)
	_draw_door(west_door)

func _draw_door_paths():
	_draw_path_door_to_point(north_door, starting_cell)
	_draw_path_door_to_point(south_door, starting_cell)
	_draw_path_door_to_point(east_door, starting_cell)
	_draw_path_door_to_point(west_door, starting_cell)

func _draw_path_between_doors(door_a: Door, door_b: Door):
	var a = door_a.enter_tile
	instructions.draw("floor", a)
	a -= door_a.enter_direction
	var b = door_b.enter_tile
	instructions.draw("floor", b)
	b -= door_b.enter_direction
	_draw_path(a, b)

func _draw_path_door_to_point(door: Door, point: Vector2i):
	var a = door.enter_tile
	instructions.draw("floor", a)
	a -= door.enter_direction
	_draw_path(a, point)

func _draw_path(a: Vector2i, b: Vector2i):
	var path = SimplePathBuilder.new().get_path_between_bidirectional(a, b)
	for coords in path:
		instructions.draw("floor", coords)

func _initialize_starting_cell():
	var starting_min: Vector2i = bounds_min + rect.size / 4
	var starting_max: Vector2i = bounds_max - rect.size / 4
	var x: int = rng.randi_range(starting_min.x, starting_max.x)
	var y: int = rng.randi_range(starting_min.y, starting_max.y)
	starting_cell = Vector2i(x, y)

func _initialize_doors():
	north_door = Door.new(north_door_pattern)
	north_door.position = Vector2i(rng.randi_range(bounds_min.x + 3, bounds_max.x - 3), bounds_min.y + 2)
	
	south_door = Door.new(south_door_pattern)
	south_door.position = Vector2i(rng.randi_range(bounds_min.x + 3, bounds_max.x - 3), bounds_max.y - 1)
	
	east_door = Door.new(east_door_pattern)
	east_door.position = Vector2i(bounds_max.x - 1, rng.randi_range(bounds_min.y + 3, bounds_max.y - 3))
	
	west_door = Door.new(west_door_pattern)
	west_door.position = Vector2i(bounds_min.x + 1, rng.randi_range(bounds_min.y + 3, bounds_max.y - 3))

