using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors;

namespace ProgJam2023.Rooms;

// Abstract class intent, do not instantiate alone
public partial class Room : Node2D
{
   [Export]
   public StringName DebugName;

   [Export]
   public Vector2I StartingCell = new Vector2I(0, 0);

   [Export]
   public TileMap TileMap { get; private set; }

   public Dictionary<Vector2I, Cell> CellMap;

   public void InitCells()
   {
      CellMap = new Dictionary<Vector2I, Cell>();

      foreach (Vector2I cell in TileMap.GetUsedCells(0))
      {
         CellMap[cell] = new Cell(cell);
      }
   }

   public void PauseAndHideRoom()
   {
      Visible = false;
   }

   public void ActivateRoom()
   {
      Visible = true;
   }

   public void PutOnCell(Vector2I cellPosition, GridActor actor)
   {
      if (!CellMap.ContainsKey(cellPosition)) return;

      if (actor.CurrentCell != null)
      {
         actor.CurrentCell.RemoveActor(actor);
      }

      Cell cell = CellMap[cellPosition];
      cell.PutActor(actor);

      actor.CurrentCell = cell;
      actor.NextCell    = cellPosition;
      actor.Position    = TileMap.ToGlobal(TileMap.MapToLocal(cellPosition) - TileMap.TileSet.TileSize / 2);
   }
}
