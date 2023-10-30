using Godot;
using Godot.Collections;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Players;
using ProgJam2023.Actors;
using System.Linq;

namespace ProgJam2023.World;

public partial class WorldManager : Node
{
   public enum State
   {
      Open,
      Busy,
   }

   public static bool DrawDebugText;
   public static Room CurrentRoom { get; private set; }
   public static Player CurrentPlayer { get; private set; }
   public static State CurrentState { get; private set; }

   static Array<GridActor> _worldActors;

   private static RoomManager _roomManager;


   public static void ChangeRoom(StringName roomKey, StringName toDoor = null)
   {
      CurrentRoom?.PauseAndHideRoom();
      SetCurrentRoom(_roomManager.Rooms[roomKey], toDoor);
   }

   public static void SetRoomManager(RoomManager roomManager)
   {
      _roomManager = roomManager;
   }

   public static void InitRooms()
   {
   }

   public static void SetCurrentRoom(Room room, StringName toDoor = null)
   {
      CurrentRoom = room;
      SpawnPlayer(toDoor);
      CurrentRoom.ActivateRoom();
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

   public static void SpawnPlayer(StringName door)
   {
      Vector2I toCell = CurrentRoom.StartingCell;

      if (door != null)
      {
         if (CurrentRoom.Doors.Keys.Contains(door)) 
         {
            toCell = CurrentRoom.Doors[door].CurrentCell.Position();
            CurrentRoom.PutOnCell(toCell, CurrentPlayer);
            TryMoveActor(CurrentPlayer, CurrentRoom.Doors[door].ExitDirection);
            return;
         }
      }

      CurrentRoom.PutOnCell(toCell, CurrentPlayer);
   }

   public static void InitWorld()
   {
      _worldActors = new Array<GridActor>();
      CurrentState = State.Open;
   }

   public static void TryMoveActor(GridActor actor, GridDirection direction)
   {
      if (actor.State != GridActor.ActorState.Idle) return;

      Vector2I toCell = actor.CurrentCell.Position() + Utils.DirectionToVector(direction);

      if (!CurrentRoom.Map.GetUsedCells(0).Contains(toCell)) return;

      if (CurrentRoom.CellMap[toCell].HasCollisions()) return;

      actor.State = GridActor.ActorState.Busy;
      actor.StepAnimation(direction);
      actor.NextCell = toCell;
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      InitWorld();
   }

   private void ProcessInput()
   {
      if (Input.IsActionJustPressed("debug11"))
      {
         DrawDebugText = !DrawDebugText;
      }
   }

   private void PlayerStep()
   {
      if (CurrentPlayer.State == GridActor.ActorState.Idle)
      {
         // Check for doors
         foreach (Door door in CurrentPlayer.CurrentCell.Actors.OfType<Door>()) 
         {
            ChangeRoom(door.ToRoom, door.ToDoor);
         }
      }
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      ProcessInput();

      PlayerStep();

      foreach (GridActor actor in _worldActors)
      {
         if (actor.State == GridActor.ActorState.NeedsUpdate)
         {
            if (actor.CurrentCell.Position() != actor.NextCell)
            {
               CurrentRoom.PutOnCell(actor.NextCell, actor);
            }
            actor.State = GridActor.ActorState.Idle;
            actor.IdleAnimation();
         }
      }
   }
}

