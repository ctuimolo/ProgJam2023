using Godot;
using Godot.Collections;

using System.Linq;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Players;
using ProgJam2023.Actors.Enemies;
using ProgJam2023.Actors;
using ProgJam2023.UI;

namespace ProgJam2023.World;

public partial class WorldManager : Node
{
   //===========================================================
   // World enums
   //===========================================================

   public enum State
   {
      Busy,
      Open,
   }

   //===========================================================
   // Debug Values
   //===========================================================

   public static bool DrawDebugText;

   //===========================================================
   // Room/PlayerState tracking
   //===========================================================

   public static Room CurrentRoom { get; private set; }
   public static StringName NextRoomKey;
   public static StringName NextRoomDoor;

   public static Player CurrentPlayer  { get; private set; }
   public static State WorldState      { get; private set; }
   public static State PlayerTurnState { get; private set; }
   public static State EnemyTurnState  { get; private set; }

   //===========================================================
   // World Collections
   //===========================================================

   static RoomManager _roomManager;

   //===========================================================
   // Turn Dynamics
   //===========================================================

   delegate void TurnProcessor();

   static TurnProcessor _playerTurnProcessor;
   static TurnProcessor _enemyTurnProcessor;

   //===========================================================
   // World transitions and UI
   //===========================================================

   public static ScreenTransitioner Transitioner = ResourceLoader.Load<PackedScene>("res://UI/ScreenTransitioner.tscn").Instantiate<ScreenTransitioner>();

   public static void ChangeRoom(StringName roomKey, StringName toDoor = null)
   {
      NextRoomKey = roomKey;
      NextRoomDoor = toDoor;

      if (CurrentRoom != null)
      {
         Transitioner.ChangeRoom_StartAnimation();
      } else
      {
         Transitioner.ChangeRoom_MovePlayer();
      }

      _playerTurnProcessor = PlayerProcess_Await;
      PlayerTurnState = State.Open;
   }

   public static void SetRoomManager(RoomManager roomManager)
   {
      _roomManager = roomManager;
   }

   public static void InitRooms()
   {
   }

   public static void SpawnPlayer(StringName room, StringName toDoor = null)
   {
      Vector2I toCell = _roomManager.Rooms[room].StartingCell;

      if (toDoor != null)
      {
         toCell = _roomManager.Rooms[room].Doors[toDoor].CurrentCell.Position();
      }

      SpawnPlayer(room, toCell);
   }

   public static void SpawnPlayer(StringName room, Vector2I toCell)
   {
      if (CurrentRoom != null)
      {
         CurrentRoom.Actors.Remove(CurrentPlayer.Name);
      }
      CurrentRoom = _roomManager.Rooms[room];
      CurrentRoom.Actors.Add(CurrentPlayer.Name, CurrentPlayer);
      CurrentRoom.PutOnCell(toCell, CurrentPlayer);
   }

   public static void SpawnPlayer_DoorAnimation(StringName door)
   {
      if (door == null) return;

      if (CurrentRoom.Doors.Keys.Contains(door))
      {
         TryMoveActor(CurrentPlayer, CurrentRoom.Doors[door].ExitDirection);
      }
   }

   public static void SetCurrentPlayer(Player player)
   {
      CurrentPlayer = player;
   }

   public void InitUI()
   {
      AddChild(Transitioner);
   }

   public static void InitWorld()
   {
      WorldState        = State.Open;
      PlayerTurnState   = State.Open;
      EnemyTurnState    = State.Open;

      _playerTurnProcessor = PlayerProcess_Await;
      _enemyTurnProcessor = EnemyProcess_Await;
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
      actor.LastStep = direction;
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      InitUI();
      InitWorld();
   }

   private void ProcessInput()
   {
      if (Input.IsActionJustPressed("debug11"))
      {
         DrawDebugText = !DrawDebugText;
      }

      if (Input.IsActionJustPressed("debug10"))
      {
         if (WorldState == State.Open)
         {
            PauseWorld();
         } else
         {
            ResumeWorld();
         }
      }
   }

   private static void EnemyProcess_Await()
   {
      // ...
   }

   private static void EnemyProcess_Process()
   {
      foreach (Enemy enemy in CurrentRoom.Actors.Values.OfType<Enemy>())
      {
         if (enemy.State == GridActor.ActorState.Idle)
         {
            enemy.TakeTurn();
         }
      }

      EnemyTurnState = State.Open;
   }

   private static void PlayerProcess_Await()
   {
      if (CurrentPlayer.ProcessInput())
      {
         _playerTurnProcessor    = PlayerProcess_Process;
         _enemyTurnProcessor     = EnemyProcess_Process;
         PlayerTurnState = State.Busy;
      }
   }

   private static void PlayerProcess_Process()
   {
      if (CurrentPlayer.State == GridActor.ActorState.Idle)
      {
         // Check for doors
         foreach (Door door in CurrentPlayer.CurrentCell.Actors.OfType<Door>()) 
         {
            ChangeRoom(door.ToRoom, door.ToDoor);
            return;
         }

         PlayerTurnState = State.Open;
      }
   }

   public static void PauseWorld()
   {
      WorldState = State.Busy;
   } 

   public static void ResumeWorld()
   {
      WorldState = State.Open;
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      ProcessInput();

      if (WorldState == State.Open)
      {
         _playerTurnProcessor.Invoke();
         _enemyTurnProcessor.Invoke();

         if (PlayerTurnState == State.Open && EnemyTurnState == State.Open) 
         {
            _playerTurnProcessor = PlayerProcess_Await;
            _enemyTurnProcessor = EnemyProcess_Await;
         }

         foreach (GridActor actor in CurrentRoom.Actors.Values)
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
}

