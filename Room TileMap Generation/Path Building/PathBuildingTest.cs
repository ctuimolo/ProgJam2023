using Godot;
using System;

using ProgJam2023.RoomDesignParameters;
using ProgJam2023.RoomTileMapGeneration;
using ProgJam2023.RoomTileMapGeneration.Paths;

public partial class PathBuildingTest : Node
{
	[Export]
	public RoomParameters Parameters;
	
	[Export]
	public RoomGenerator RoomGenerator;
	
	public override void _Ready()
	{
		RoomGenerator.Parameters = Parameters;
		RoomGenerator.Generate();
	}
}
