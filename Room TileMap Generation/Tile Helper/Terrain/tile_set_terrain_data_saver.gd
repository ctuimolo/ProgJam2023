class_name TileSetTerrainDataSaver extends Node

@export var tile_set: TileSet
@export var path: String

@export var terrain: int
@export var terrain_index: int

@export var save_on_run: bool

# Can't save with a button in the inspector
# A bug prevents reading terrain peering bits while in edit mode
# Add this script to a scene and run to save the resource

func _ready():
	if save_on_run:
		save()

func save():
	print("Saving terrain data: ", path)
	_save_resource(_get_tile_terrain_data_collection())

func _get_tile_terrain_data_collection()->TileTerrainDataCollection:
	var result: TileTerrainDataCollection = TileTerrainDataCollection.new()
	result.terrain = terrain
	result.terrain_index = terrain_index
	for tile in TileSetHelper.new().get_all_tiles_in_set(tile_set):
		result.terrain_data.append(TileTerrainData.new(tile))
	return result


func _fix_path(p_path: String)->String:
	assert(p_path != null && len(p_path) > 0)
	var result: String = p_path
	if !result.begins_with("res://"):
		result = "res://" + result
	if result.ends_with(".tres"):
		result = result.substr(0, len(result) - 5)
	return result

func _save_resource(resource: TileTerrainDataCollection):
	var p: String = _fix_path(path)
	ResourceSaver.save(resource, p + ".tres")
