class_name SimplePathBuilder extends RefCounted

func get_path_between(a: Vector2i, b: Vector2i)->Array:
	var difference = b - a
	var step_x = Vector2i(1 if difference.x > 0 else -1, 0)
	var step_y = Vector2i(0, 1 if difference.y > 0 else -1)
	var coords = a
	var result: Array[Vector2i] = []
	while coords != b:
		result.append(coords)
		if coords.x == b.x:
			coords += step_y
		elif coords.y == b.y:
			coords += step_x
		elif randi_range(0, 1) == 0:
			coords += step_y
		else:
			coords += step_x
	result.append(b)
	return result

func get_path_between_bidirectional(a: Vector2i, b: Vector2i)->Array:
	var difference = b - a
	var step_x = Vector2i(1 if difference.x > 0 else -1, 0)
	var step_y = Vector2i(0, 1 if difference.y > 0 else -1)
	var a_coords = a
	var b_coords = b
	var a_path: Array[Vector2i] = [ a ]
	var b_path: Array[Vector2i] = [ b ]
	while a_coords != b_coords:
		# Step a
		if a_coords.x == b_coords.x:
			a_coords += step_y
		elif a_coords.y == b_coords.y:
			a_coords += step_x
		elif randi_range(0, 1) == 0:
			a_coords += step_y
		else:
			a_coords += step_x
		if a_coords == b_coords:
			break
		else:
			a_path.append(a_coords)
		# Step b
		if a_coords == b_coords:
			break
		if a_coords.x == b_coords.x:
			b_coords -= step_y
		elif a_coords.y == b_coords.y:
			b_coords -= step_x
		elif randi_range(0, 1) == 0:
			b_coords -= step_y
		else:
			b_coords -= step_x
		if a_coords == b_coords:
			break
		else:
			b_path.append(b_coords)
	var result: Array[Vector2i] = []
	result.append_array(a_path)
	b_path.reverse()
	result.append_array(b_path)
	return result
	
