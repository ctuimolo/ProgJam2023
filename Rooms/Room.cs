using Godot;
using ProgJam2023.Actors;
using ProgJam2023.World;
using System;
using System.Linq;

namespace ProgJam2023.Rooms;

// Abstract class intent, do not instantiate alone
public partial class Room : Node2D
{
   [Export]
   public Vector2I StartingCell = new Vector2I(0, 0);

   [Export]
   public TileMap TileMap { get; private set; }

   // Probably to be called in Godot scene room's _Ready function
   protected void InitRoom()
   {
      WorldManager.SetCurrentRoom(this);
      WorldManager.SpawnPlayer();
   }

   public void PutOnCell(Vector2I cell, GridActor actor)
   {
      if (!TileMap.GetUsedCells(0).Contains(cell)) return;

      actor.CurrentCell = cell;
      actor.NextCell = cell;
      actor.Position = TileMap.ToGlobal(TileMap.MapToLocal(cell) - TileMap.TileSet.TileSize / 2);
   }
}
