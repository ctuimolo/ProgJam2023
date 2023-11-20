using Godot;
using System;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class EnemySet : Resource
{
	[Export]
	public PackedScene[] Enemies;
}
