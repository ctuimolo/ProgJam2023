@tool

class_name TerrainToRulesTileMap extends Node

@export var tile_map: TileMap
@export var terrain_data: TileTerrainDataCollection

@export var row_length: int = 100

@export var populate: bool = false
@export var clear: bool = false

class Pair:
	var a: Tile
	var b: Tile
	var direction: TileSet.CellNeighbor
	func _init(p_a: Tile, p_b: Tile, p_direction: TileSet.CellNeighbor):
		a = p_a
		b = p_b
		direction = p_direction
	
var cell_neighbor_to_vector: Dictionary = {
	TileSet.CELL_NEIGHBOR_TOP_SIDE: Vector2i(0, -1),
	TileSet.CELL_NEIGHBOR_RIGHT_SIDE: Vector2i(1, 0)
}

func _process(delta):
	if clear:
		clear = false
		tile_map.clear()
	if populate:
		populate = false
		populate_tile_map()

func populate_tile_map():
	var vertical_pairs: Array[Pair] = get_pairs(TileSet.CELL_NEIGHBOR_TOP_SIDE)
	var x: int = 0
	var y: int = 0
	for pair in vertical_pairs:
		draw_vertical_pair(pair, Vector2i(x, y))
		x += 2
		if x > row_length:
			x = 0
			y += 3
	if x > 0:
		x = 0
		y += 3
	var horizontal_pairs: Array[Pair] = get_pairs(TileSet.CELL_NEIGHBOR_RIGHT_SIDE)
	for pair in horizontal_pairs:
		draw_horizontal_pair(pair, Vector2i(x, y))
		x += 3
		if x > row_length:
			x = 0
			y += 2

func get_pairs(direction: TileSet.CellNeighbor)->Array[Pair]:
	var pairs: Array[Pair] = []
	for data1 in terrain_data.terrain_data:
		for data2 in terrain_data.terrain_data:
			if _check_tiles_match(data1, data2, direction):
				pairs.append(Pair.new(data1.tile, data2.tile, direction))
	return pairs

func _check_tiles_match(data1: TileTerrainData, data2: TileTerrainData, direction: TileSet.CellNeighbor)->bool:
	match direction:
		TileSet.CELL_NEIGHBOR_TOP_SIDE:
			return data1.top_side_peers_with(data2)
		TileSet.CELL_NEIGHBOR_RIGHT_SIDE:
			return data1.right_side_peers_with(data2)
		TileSet.CELL_NEIGHBOR_BOTTOM_SIDE:
			return data1.bottom_side_peers_with(data2)
		TileSet.CELL_NEIGHBOR_LEFT_SIDE:
			return data1.left_side_peers_with(data2)
		_:
			assert(false)
			return false

func draw_vertical_pair(pair: Pair, coords: Vector2i):
	var a: TileAttributes = pair.a.attributes
	var b: TileAttributes = pair.b.attributes
	tile_map.set_cell(0, coords + Vector2i(0, 1), a.source_id, a.atlas_coords,a.alternative_tile)
	tile_map.set_cell(0, coords, b.source_id, b.atlas_coords,b.alternative_tile)

func draw_horizontal_pair(pair: Pair, coords: Vector2i):
	var a: TileAttributes = pair.a.attributes
	var b: TileAttributes = pair.b.attributes
	tile_map.set_cell(0, coords, a.source_id, a.atlas_coords,a.alternative_tile)
	tile_map.set_cell(0, coords + Vector2i(1, 0), b.source_id, b.atlas_coords,b.alternative_tile)
