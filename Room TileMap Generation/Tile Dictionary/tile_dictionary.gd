class_name TileDictionary extends Resource

@export var pairs: Array[TileDictionaryPair]
var _dictionary

var _initialized: bool = false

func _init():
	pass

func initialize():
	if _initialized:
		return
	_initialized = true
	_dictionary = { }
	for pair in pairs:
		_dictionary[pair.key] = pair.value

func get_tile(key: String)->TileAttributes:
	assert(_initialized)
	if !_dictionary.has(key):
		return null
	return _dictionary[key]
