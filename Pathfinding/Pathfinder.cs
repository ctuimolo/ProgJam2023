using Godot;
using System;
using System.Collections.Generic;

using ProgJam2023.Rooms;

namespace ProgJam2023.Pathfinding;

public abstract class Pathfinder
{
	// Make a path from start to end
	public abstract Path GetPath(Cell start, Cell end);
	
	// Make sure path is not blocked
	public virtual bool PathIsValid(Path path) {
		return PathIsValid(path, path.Count);
	}
	public abstract bool PathIsValid(Path path, int steps = -1);
}
