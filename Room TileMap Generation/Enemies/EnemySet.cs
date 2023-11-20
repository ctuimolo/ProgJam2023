using Godot;
using System;

using ProgJam2023.RoomTileMapGeneration;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class EnemySet : Resource
{
	[Export]
	public SpawnableEnemy[] Enemies;
}
