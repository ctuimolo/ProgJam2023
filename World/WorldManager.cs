using Godot;
using Godot.Collections;

using System.Linq;

using ProgJam2023.Rooms;
using ProgJam2023.Actors.Players;
using ProgJam2023.Actors;
using ProgJam2023.UI;
using System.Threading;

namespace ProgJam2023.World;

public partial class WorldManager : Node
{
   public enum State
   {
      Busy,
      Open,
   }

   public static bool DrawDebugText;
   public static Room CurrentRoom { get; private set; }
   public static Player CurrentPlayer { get; private set; }
   public static State PlayerTurnState { get; private set; }
   public static State EnemyTurnState { get; private set; }

   static Array<GridActor> _worldActors;

   private static RoomManager _roomManager;

   private delegate void TurnProcessor();

   private static TurnProcessor _playerTurnProcessor;
   private static TurnProcessor _enemyTurnProcessor;

   public static ScreenTransitioner Transitioner = ResourceLoader.Load<PackedScene>("res://UI/ScreenTransitioner.tscn").Instantiate<ScreenTransitioner>();

   public static async void ChangeRoom(StringName roomKey, StringName toDoor = null)
   {
      if (CurrentRoom != null)
      {
         //Transitioner.FadeOut();
         //await Transitioner.WaitOnAnimation();
         //Transitioner.Clear();
      }
      
      CurrentRoom?.PauseAndHideRoom();

      //Transitioner.FadeIn();
      //await Transitioner.WaitOnAnimation();

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

   public void InitUI()
   {
      AddChild(Transitioner);
   }

   public static void InitWorld()
   {
      _worldActors      = new Array<GridActor>();
      PlayerTurnState   = State.Open;
      EnemyTurnState    = State.Open;

      _playerTurnProcessor = PlayerProcess_Open;
      _enemyTurnProcessor = EnemyProcess_Open;
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
      InitUI();
      InitWorld();
   }

   private void ProcessInput()
   {
      if (Input.IsActionJustPressed("debug11"))
      {
         DrawDebugText = !DrawDebugText;
      }
   }

   private static void EnemyProcess_Open()
   {
   }

   private static void EnemyProcess_Busy()
   {
   }

   private static void PlayerProcess_Open()
   {
      if (CurrentPlayer.ProcessInput())
      {
         _playerTurnProcessor = PlayerProcessEnd_Busy;
         PlayerTurnState = State.Busy;
      }
   }

   private static void PlayerProcessEnd_Busy()
   {
      if (CurrentPlayer.State == GridActor.ActorState.Idle)
      {
         // Check for doors
         foreach (Door door in CurrentPlayer.CurrentCell.Actors.OfType<Door>()) 
         {
            ChangeRoom(door.ToRoom, door.ToDoor);
            break;
         }

         _playerTurnProcessor = PlayerProcess_Open;
         PlayerTurnState = State.Open;
      }
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      ProcessInput();

      _playerTurnProcessor.Invoke();
      _enemyTurnProcessor.Invoke();

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

