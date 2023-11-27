using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

[GlobalClass]
public partial class SpawnableEnemy : Resource
{
	[Export]
	public ProgJam2023.Rooms.EnemySpawner.EnemyType Type;
	
	// Check if the enemy can spawn at this position
	public virtual bool CanSpawn(Vector2I cell, RoomTileMapEditor tileMapEditor)
	{
		if(tileMapEditor.ClaimedCellCollection.CellIsClaimed(cell))
		{
			return false;
		}
		return tileMapEditor.CellIsNavigable(cell);
	}
	
	// Draw tiles/instructions and claim cells required for the enemy to spawn at the position
	public virtual void PrepareSpawnArea(Vector2I cell, RoomTileMapEditor tileMapEditor)
	{
		tileMapEditor.DrawInstruction(cell, "floor", "Enemy");
	}
}
