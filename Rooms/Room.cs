using Godot;

using System;
using System.Collections.Generic;
using System.Linq;

using ProgJam2023.Actors;
using ProgJam2023.Actors.Enemies;

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

   List<EnemySpawner.Instruction> EnemySpawnInstructions = new List<EnemySpawner.Instruction>();

   public void InitCells()
   {
      foreach (Vector2I cell in Map.GetUsedCells(0))
      {
         CellMap[cell] = new Cell(cell, this);
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

   public void PutOnCell(Vector2I cellPosition, GridActor actor, bool updatePositionDraw = false)
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

      if (updatePositionDraw)
      {
         UpdateCellPositionDraw(actor);
      }
   }

   public void UpdateCellPositionDraw(GridActor actor)
   {
      actor.Position = Map.MapToLocal(actor.CurrentCell.Position()) - Map.TileSet.TileSize / 2;
   }

   public void FindAndAddActors()
   {
      // Spawning from PackedScene
      foreach (GridActor actor in GetChild(0).GetChildren().OfType<GridActor>())
      {
         if (actor is Door)
         {
            Doors.Add(actor.Name, actor as Door);
         }

         Actors.Add(actor.Name, actor);
         Vector2I toCell = Map.LocalToMap(actor.Position);
         PutOnCell(toCell, actor, true);

         actor.CurrentRoom = this;
      }

      // Enemy Spawning
      foreach (EnemySpawner.Instruction instruction in EnemySpawnInstructions)
      {
         Enemy enemy = EnemySpawner.GetEnemyScene(instruction.Type).Instantiate<Enemy>();
         AddChild(enemy);
         PutOnCell(instruction.Cell, enemy, true);
         Actors.Add(enemy.Name, enemy);

         enemy.CurrentRoom = this;
      }
   }
   
   public void AddEnemySpawnInstruction(EnemySpawner.Instruction instruction)
   {
      EnemySpawnInstructions.Add(instruction);
   }

   private void ParseDebug_EnemySpawnCommands()
   {
      if (Map.EnemySpawnCommands_Debug != null)
      {
         string[] lines = Map.EnemySpawnCommands_Debug.Split(';');

         foreach (string line in lines)
         {
            string[] arg = line.Trim().Trim('\n').Split(' ');

            if (arg.Length < 4) continue;

            string enemyType = arg[0];
            int spawnX = arg[1].ToInt();
            int spawnY = arg[2].ToInt();
            string name = arg[3].ToString();

            foreach (EnemySpawner.EnemyType type in Enum.GetValues(typeof(EnemySpawner.EnemyType)))
            {
               if (enemyType == type.ToString())
               {
                  EnemySpawnInstructions.Add(new EnemySpawner.Instruction()
                  {
                     Type = type,
                     Cell = new Vector2I(spawnX, spawnY),
                     Name = name,
                  });
               }
            }
         }
      }
   }

   public override void _Ready()
   {
      ParseDebug_EnemySpawnCommands();
   }
}
