using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.RoomTileMapGeneration;

public partial class RoomWFCManager : Node
{
	[Export]
	private Node GDRoomWFCManager;
	
	public TileMap TargetTileMap => (TileMap)GDRoomWFCManager.Get("target_tile_map");
	
	[Signal]
	public delegate void TilesFinishedEventHandler();
	
	[Signal]
	public delegate void GenerationErrorEventHandler();
	
	public void Collapse()
	{
		GDRoomWFCManager.Call("collapse");
	}
	
	public void SetRect(Rect2I rect)
	{
		GDRoomWFCManager.Call("set_rect", rect);
	}
	
	public void SetTileSubsetWhitelist(StringName[] whitelist)
	{
		Godot.Collections.Array godot_array = Variant.From(whitelist).AsGodotArray();
		GDRoomWFCManager.Call("set_tile_subset_whitelist", godot_array);
	}
	
	public void SetTargetTileMap(TileMap tileMap)
	{
		GDRoomWFCManager.Call("set_target_tile_map", tileMap);
	}
	public void SetInstructionTileMap(InstructionTileMap instructionTileMap)
	{
		GDRoomWFCManager.Call("set_instruction_tile_map", instructionTileMap.GDInstructionTileMap);
	}
	
	private void OnTilesFinished()
	{
		EmitSignal(SignalName.TilesFinished);
	}
	
	private void OnGenerationError()
	{
		EmitSignal(SignalName.GenerationError);
	}
}
