using Godot;
using Godot.Collections;

using ProgJam2023.Actors;
using ProgJam2023.World;

namespace ProgJam2023.Rooms;

// Abstract class intent, do not instantiate alone
public partial class Room : Node2D
{
   [Export]
   public Vector2I StartingCell = new Vector2I(0, 0);

   [Export]
   public TileMap TileMap { get; private set; }

   [Export]
   Array<Door> _doors = new Array<Door>();

   public void PauseAndHideRoom()
   {
      Visible = false;
   }

   public void ActivateRoom()
   {
      Visible = true;
   }

   public void InitRoom()
   {
      Visible = true;
      WorldManager.SpawnPlayer(StartingCell, GridDirection.None);
   }

   public void PutOnCell(Vector2I cell, GridActor actor)
   {
      if (!TileMap.GetUsedCells(0).Contains(cell)) return;

      actor.CurrentCell = cell;
      actor.NextCell = cell;
      actor.Position = TileMap.ToGlobal(TileMap.MapToLocal(cell) - TileMap.TileSet.TileSize / 2);
   }
}
