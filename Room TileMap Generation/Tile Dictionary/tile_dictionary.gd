class_name TileDictionary extends Resource

@export var pairs: Array[TileDictionaryPair]
var _dictionary
var _reverse_dictionary

var _initialized: bool = false

func _init():
	pass

func initialize():
	if _initialized:
		return
	_initialized = true
	_dictionary = { }
	_reverse_dictionary = { }
	for pair in pairs:
		_dictionary[pair.key] = pair.value
		_reverse_dictionary[pair.value] = pair.key

func get_tile(key: String)->TileAttributes:
	assert(_initialized)
	if !_dictionary.has(key):
		return null
	return _dictionary[key]

func get_key(value: TileAttributes)->String:
	assert(_initialized)
	if !_reverse_dictionary.has(value):
		return ""
	return _reverse_dictionary[value]
	
