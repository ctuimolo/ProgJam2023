@tool

class_name TilePatternSaver extends Node

@export var tile_set: TileSet
@export var path: String
@export var save: bool = false

func _process(delta):
	if save:
		save = false
		_save_patterns()

func _save_patterns():
	assert(path != null && len(path) > 0)
	var p: String = path
	if !p.begins_with("res://"):
		p = "res://" + p
	if p.ends_with(".tres"):
		p = p.substr(0, len(p) - 5)
	var test: TileAttributes = TileAttributes.new()
	#ResourceSaver.save(test, p + ".tres")
	for i in range(tile_set.get_patterns_count()):
		var pattern: TileMapPattern = tile_set.get_pattern(i)
		ResourceSaver.save(pattern, p + " " + str(i) + ".tres")
