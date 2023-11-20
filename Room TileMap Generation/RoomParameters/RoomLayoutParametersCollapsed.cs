using Godot;
using System;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class RoomLayoutParametersCollapsed : Resource
{
	public Vector2I Size;
	public bool IsSquare;
	
	public RoomLayoutParametersCollapsed() { }
	public RoomLayoutParametersCollapsed(RoomLayoutParametersCollapsed other)
	{
		Size = other.Size;
		IsSquare = other.IsSquare;
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{ ");
		
		stringBuilder.Append($"Size: {Size}, ");
		stringBuilder.Append($"IsSquare: {IsSquare}");
		
		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}
}
