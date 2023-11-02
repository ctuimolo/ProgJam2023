using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors;
using System.Security.Cryptography;
using System.Linq;

namespace ProgJam2023.Rooms;

// Abstract class intent, do not instantiate alone
public partial class Room : Node2D
{
   [Export]
   public Vector2I StartingCell = new Vector2I(0, 0);

   [Export]
   public RoomTileMap Map { get; set; }

   public Dictionary<Vector2I, Cell> CellMap = new Dictionary<Vector2I, Cell>();
   public Dictionary<StringName, Door> Doors = new Dictionary<StringName, Door>();
   public Dictionary<StringName, GridActor> Actors = new Dictionary<StringName, GridActor>();

   public void InitCells()
   {
      foreach (Vector2I cell in Map.GetUsedCells(0))
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

      if (actor.GetParent() != Map)
      {
         actor.Reparent(Map);
      }

      if (actor.CurrentCell != null)
      {
         actor.CurrentCell.RemoveActor(actor);
      }

      Cell cell = CellMap[cellPosition];
      cell.PutActor(actor);

      actor.CurrentCell = cell;
      actor.NextCell    = cellPosition;
      actor.Position    = Map.MapToLocal(cellPosition) - Map.TileSet.TileSize / 2;
   }

   public void FindAndAddActors(bool setGlobalToCell = false)
   {
      foreach (GridActor actor in GetChild(0).GetChildren().OfType<GridActor>())
      {
         if (actor is Door)
         {
            Doors.Add(actor.Name, actor as Door);
         }

         Actors.Add(actor.Name, actor);
         if (setGlobalToCell)
         {
            Vector2I toCell = Map.LocalToMap(actor.Position);
            PutOnCell(toCell, actor);
         }
      }
   }


   public override void _Ready()
   {
   }
}
