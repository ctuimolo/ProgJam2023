using Godot;
using System;
using System.Collections.Generic;

namespace ProgJam2023.Rooms;

public partial class RoomTileMap : TileMap
{
   [Export]
   public StringName DebugName;

   [Export]
   public string EnemySpawnCommands_Debug;
}
