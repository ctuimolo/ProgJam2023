using Godot;
using Godot.Collections;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Player;
using ProgJam2023.Actors;

namespace ProgJam2023.World;

public partial class WorldManager : Node
{
   public enum State
   {
      Open,
      Busy,
   }

   public static Room CurrentRoom { get; private set; }
   public static Player CurrentPlayer { get; private set; }
   public static State CurrentState { get; private set; }

   static Array<GridActor> _worldActors;

   public static void ChangeRoom(Room toRoom)
   {
      CurrentRoom?.PauseAndHideRoom();
      
      if (toRoom == null)
      {
         return;
      }

      SetCurrentRoom(toRoom);
      CurrentRoom.InitRoom();
   }

   public static void SetCurrentRoom(Room room)
   {
      CurrentRoom = room;
   }

   public static void SetCurrentPlayer(Player player)
   {
      CurrentPlayer = player;
      AddActorToWorld(player);
   }

   public static void AddActorToWorld(GridActor actor)
   {
      if (_worldActors.Contains(actor))
      {
         return;
      }

      _worldActors.Add(actor);
   }

   public static void SpawnPlayer(Vector2I cell, GridDirection walkIn)
   {
      CurrentRoom.PutOnCell(cell, CurrentPlayer);
   }

   public static void InitWorld()
   {
      _worldActors = new Array<GridActor>();
      CurrentState = State.Open;
   }

   public static void TryMoveActor(GridActor actor, GridDirection direction)
   {
      if (actor.State != GridActor.ActorState.Idle) return;

      Vector2I toCell = actor.CurrentCell + Utils.DirectionToVector(direction);

      if (!CurrentRoom.TileMap.GetUsedCells(0).Contains(toCell)) return;

      actor.State = GridActor.ActorState.Busy;
      actor.StepAnimation(direction);
      actor.NextCell = toCell;
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      InitWorld();
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      foreach (GridActor actor in _worldActors)
      {
         if (actor.State == GridActor.ActorState.NeedsUpdate)
         {
            if (actor.CurrentCell != actor.NextCell)
            {
               CurrentRoom.PutOnCell(actor.NextCell, actor);
            }
            actor.State = GridActor.ActorState.Idle;
            actor.IdleAnimation();
         }
      }
   }
}

