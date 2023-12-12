@tool

class_name TileTerrainDataCollection extends Resource

@export var terrain_data: Array[TileTerrainData]
@export var terrain: int
@export var terrain_index: int

func _init():
	terrain_data = []
	terrain = -1
	terrain_index = -1
