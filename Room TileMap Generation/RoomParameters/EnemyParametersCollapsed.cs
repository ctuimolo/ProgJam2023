using Godot;
using System;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class EnemyParametersCollapsed : Resource
{
	
	[Export]
	public PackedScene[] Enemies;
	
	public EnemyParametersCollapsed()
	{
		Enemies = new PackedScene[0];
	}
	public EnemyParametersCollapsed(EnemyParametersCollapsed other) : this()
	{
		Enemies = new PackedScene[other.Enemies.Length];
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
