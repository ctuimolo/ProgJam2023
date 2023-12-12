@tool

class_name TileTerrainData extends Resource

@export var tile: Tile
@export var peering_bits: Array[int]

func _init(p_tile: Tile = null):
	tile = p_tile
	peering_bits = get_empty_peering_bits()
	if tile == null:
		return
	for cell_neighbor in range(16):
		var bit: int = tile.data.get_terrain_peering_bit(cell_neighbor)
		peering_bits[cell_neighbor] = bit

func get_empty_peering_bits()->Array[int]:
	return [
		0, #  0 Right side
		0, #  1 Right corner
		0, #  2 Bottom right side
		0, #  3 Bottom right corner
		0, #  4 Bottom side
		0, #  5 Bottom corner
		0, #  6 Bottom left side
 		0, #  7 Bottom left corner
		0, #  8 Left side
		0, #  9 Left corner
		0, # 10 Top left side
		0, # 11 Top left corner
		0, # 12 Top side
		0, # 13 Top corner
		0, # 14 Top right side
		0, # 15 Top right corner
	]

const RIGHT: int = 0
const BOTTOM_RIGHT: int = 3
const BOTTOM: int = 4
const BOTTOM_LEFT: int = 7
const LEFT: int = 8
const TOP_LEFT: int = 11
const TOP: int = 12
const TOP_RIGHT: int = 15

func top_side_peers_with(other: TileTerrainData)->bool:
	var result: bool = true
	result = result && _compare_bits(peering_bits[TOP_LEFT], other.peering_bits[BOTTOM_LEFT], true)
	result = result && _compare_bits(peering_bits[TOP], other.peering_bits[BOTTOM], false)
	result = result && _compare_bits(peering_bits[TOP_RIGHT], other.peering_bits[BOTTOM_RIGHT], true)
	return result

func right_side_peers_with(other: TileTerrainData)->bool:
	var result: bool = true
	result = result && _compare_bits(peering_bits[TOP_RIGHT], other.peering_bits[TOP_LEFT], true)
	result = result && _compare_bits(peering_bits[RIGHT], other.peering_bits[LEFT], false)
	result = result && _compare_bits(peering_bits[BOTTOM_RIGHT], other.peering_bits[BOTTOM_LEFT], true)
	return result

func bottom_side_peers_with(other: TileTerrainData)->bool:
	return other.top_side_peers_with(self)

func left_side_peers_with(other: TileTerrainData)->bool:
	return other.right_side_peers_with(self)

func _compare_bits(bit1: int, bit2: int, can_be_negative_one: bool)->bool:
	return bit1 == bit2 && (bit1 != -1 || can_be_negative_one)

#func peers_with(other: TileTerrainData, neighbor: TileSet.CellNeighbor)->bool:
#	assert(len(peering_bits) == 16 && len(other.peering_bits) == 16)
#	var opposite = neighbor_to_opposite[neighbor]
#	return peering_bits[neighbor] == other.peering_bits[opposite]
