using Godot;
using System;

using static RandomSelection;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class RoomParameters : Resource
{
	[Export]
	public RoomData Data;
	
	[Export]
	public RoomLayoutParameters Layout;
	
	[Export]
	public EnemyParameters Enemies;
	
	public RoomParameters()
	{
		Data = new RoomData();
		Layout = new RoomLayoutParameters();
		Enemies = new EnemyParameters();
	}
	public RoomParameters(RoomParameters other) : this()
	{
		Data = new RoomData(other.Data);
		Layout = new RoomLayoutParameters(other.Layout);
		Enemies = new EnemyParameters(other.Enemies);
	}
	
	public RoomParametersCollapsed GetCollapsed(RandomNumberGenerator rng)
	{
		RoomParametersCollapsed collapsed = new RoomParametersCollapsed();
		collapsed.Data = Data;
		collapsed.Layout = Layout.GetCollapsed(rng);
		collapsed.Enemies = Enemies.GetCollapsed(rng);
		return collapsed;
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{ ");
		
		stringBuilder.Append($"Data: {Data}, ");
		stringBuilder.Append($"Layout: {Layout}, ");
		stringBuilder.Append($"Enemies: {Enemies}");
		
		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}
	
}
