using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

[GlobalClass]
public partial class SpawnableEnemy : Resource
{
	[Export]
	public ProgJam2023.Rooms.EnemySpawner.EnemyType Type;
	
	public virtual bool CanSpawn(Vector2I cell, RoomDrawer drawer)
	{
		return drawer.CellIsNavigable(cell);
	}
}
