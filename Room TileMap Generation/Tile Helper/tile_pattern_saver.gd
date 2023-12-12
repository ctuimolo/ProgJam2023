@tool

class_name TilePatternSaver extends Node

@export var tile_set: TileSet
@export var path: String
@export var save: bool = false

func _process(delta):
	if save:
		save = false
		_save_patterns()

func _fix_path(p_path: String)->String:
	assert(p_path != null && len(p_path) > 0)
	var result: String = p_path
	if !result.begins_with("res://"):
		result = "res://" + result
	if result.ends_with(".tres"):
		result = result.substr(0, len(result) - 5)
	return result

func _save_patterns():
	var p: String = _fix_path(path)
	for i in range(tile_set.get_patterns_count()):
		var pattern: TileMapPattern = tile_set.get_pattern(i)
		ResourceSaver.save(pattern, p + " " + str(i) + ".tres")
