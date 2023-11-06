class_name DoorPattern extends Resource

@export var tile_map_pattern: TileMapPattern
@export var center: Vector2i
## Direction player would move to enter the door and leave the room
@export var enter_direction: Vector2i

func _init(p_tile_map_pattern: TileMapPattern = null, p_center: Vector2i = Vector2i(0, 0), p_enter_direction: Vector2i = Vector2i(0, 0)):
	tile_map_pattern = p_tile_map_pattern
	center = p_center
	enter_direction = p_enter_direction
