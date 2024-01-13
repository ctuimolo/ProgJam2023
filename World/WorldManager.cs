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
   public static State EnemyTurnState()
   {
      if (CurrentRoom == null) return State.Open;
      if (CurrentRoom.Actors == null) return State.Open;

      foreach (GridActor actor in CurrentRoom.Actors.Values) 
      {
         if (actor.State != GridActor.ActorState.Idle)
         {
            return State.Busy;
         }
      }
      return State.Open;
   }

   public static Player.Instruction PlayerInstruction = null;

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

      _playerTurnProcessor = PlayerProcess_AwaitInstruction;
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
      CurrentPlayer.InitPlayer();
      CurrentRoom = _roomManager.Rooms[room];
      CurrentRoom.Actors.Add(CurrentPlayer.Name, CurrentPlayer);
      CurrentRoom.PutOnCell(toCell, CurrentPlayer, true);
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

   public static void SetPlayerInstruction(Player.Instruction instruction)
   {
      PlayerInstruction = instruction;
   }

   private static void ConsumePlayerInstruction()
   {
      if (PlayerInstruction != null)
      {
         switch(PlayerInstruction.Type)
         {
            case Player.InstructionType.Move:
               TryMoveActor(CurrentPlayer, PlayerInstruction.Direction);
               break;
         }
      }

      PlayerInstruction = null;
   }

   public void InitUI()
   {
      AddChild(Transitioner);
   }

   public static void InitWorld()
   {
      WorldState        = State.Open;
      PlayerTurnState   = State.Open;

      _playerTurnProcessor = PlayerProcess_AwaitInstruction;
      _enemyTurnProcessor = EnemyProcess_Await;
   }

   public static bool TestTraversable(GridActor actor, Vector2I cell)
   {
      if (!CurrentRoom.CellMap.ContainsKey(cell)) return false;
      return !CurrentRoom.CellMap[cell].HasCollisions(actor is not Enemy);
   }

   public static bool TestTraversable(GridActor actor, GridDirection direction)
   {
      Vector2I toCell = actor.CurrentCell.Position() + Utils.DirectionToVector(direction);
      if (!CurrentRoom.CellMap.ContainsKey(toCell)) return false;
      return !CurrentRoom.CellMap[toCell].HasCollisions(actor is not Enemy);
   }
   public static bool TestTraversable(GridActor actor, Cell cell)
   {
      if(!CurrentRoom.CellMap.TryGetValue(cell.Position(), out Cell roomCell) || cell != roomCell)
      {
         return false;
      }
      return !cell.HasCollisions(actor is not Enemy);
   }

   public static bool TryMoveActor(GridActor actor, GridDirection direction)
   {
      if (actor.State != GridActor.ActorState.Idle) return false;

      Vector2I toCell = actor.CurrentCell.Position() + Utils.DirectionToVector(direction);

      if (!TestTraversable(actor, toCell)) return false;

      actor.State = GridActor.ActorState.Busy;
      actor.StepAnimation(direction);
      actor.LastStep = direction;

      CurrentRoom.PutOnCell(toCell, actor, false);

      return true;
   }

   public static bool TryMoveActorToCell(GridActor actor, Cell cell)
   {
      if (actor.State != GridActor.ActorState.Idle) return false;
      if (!TestTraversable(actor, cell.Position())) return false;

      actor.State = GridActor.ActorState.Busy;
      //actor.StepAnimation(direction);
      //actor.LastStep = direction;

      CurrentRoom.PutOnCell(cell.Position(), actor, false);

      return true;
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
      foreach (Enemy enemy in CurrentRoom.Actors.Values.OfType<Enemy>())
      {
         if (enemy.State == GridActor.ActorState.Idle)
         {
            enemy.Idle();
         }
      }
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
   }

   private static void PlayerProcess_AwaitInstruction()
   {
      if (CurrentPlayer.ProcessInput())
      {
         PlayerTurnState = State.Busy;
         _playerTurnProcessor = PlayerProcess_ProcessInstruction;
      }
   }

   private static void PlayerProcess_ProcessInstruction()
   {
      ConsumePlayerInstruction();

      _playerTurnProcessor = PlayerProcess_Finish;
      _enemyTurnProcessor  = EnemyProcess_Process;
   }

   public static void PlayerProcess_Finish()
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
         _playerTurnProcessor = PlayerProcess_AwaitInstruction;
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

         if (PlayerTurnState == State.Open && EnemyTurnState() == State.Open) 
         {
            _playerTurnProcessor = PlayerProcess_AwaitInstruction;
            _enemyTurnProcessor  = EnemyProcess_Await;
         }

         foreach (GridActor actor in CurrentRoom.Actors.Values)
         {
            if (actor.State == GridActor.ActorState.NeedsUpdate)
            {
               CurrentRoom.UpdateCellPositionDraw(actor);
               actor.State = GridActor.ActorState.Idle;
               actor.IdleAnimation();
            }
         }
      }
   }
}

