using Godot;
using System;

using ProgJam2023.RoomTileMapGeneration;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class EnemyParametersCollapsed : Resource
{
	
	[Export]
	public SpawnableEnemy[] Enemies;
	
	public EnemyParametersCollapsed()
	{
		Enemies = new SpawnableEnemy[0];
	}
	public EnemyParametersCollapsed(EnemyParametersCollapsed other) : this()
	{
		Enemies = new SpawnableEnemy[other.Enemies.Length];
		Array.Copy(other.Enemies, Enemies, Enemies.Length);
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{");
		
		stringBuilder.Append($"Enemies: {Enemies}");
		
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}
	
}
