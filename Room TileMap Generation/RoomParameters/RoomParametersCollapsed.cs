using Godot;
using System;

using static RandomSelection;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class RoomParametersCollapsed : Resource
{
	public RoomData Data;
	public RoomLayoutParametersCollapsed Layout;
	public EnemyParametersCollapsed Enemies;
	
	public RoomParametersCollapsed() { }
	public RoomParametersCollapsed(RoomParametersCollapsed other) : this()
	{
		Data = new RoomData(other.Data);
		Layout = new RoomLayoutParametersCollapsed(other.Layout);
		Enemies = new EnemyParametersCollapsed(other.Enemies);
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
