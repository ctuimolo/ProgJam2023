using Godot;

using ProgJam2023.World;
using ProgJam2023.Dynamics;

namespace ProgJam2023.Actors.Players;




public partial class Player : GridActor
{
   public enum InstructionType
   {
      Move,
   }

   public class Instruction
   {
      public InstructionType Type;
      public GridDirection Direction;
   }

   [Export]
   protected FrameTimer _directionalInputTimer;

   private GridDirection _lastDirection = GridDirection.None;
   private GridDirection _direction = GridDirection.None;
   private int _directionHoldCount = 0;
   private int _directionHoldTime = 6;

   // Probably to be called in Godot scene room's _Ready function
   public void InitPlayer()
   {
      _animationWalker.SpeedScale = 2.4f;
      _directionHoldCount = 0;

      WorldManager.SetCurrentPlayer(this);
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      base._Ready();
      InitPlayer();
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      base._Process(delta);
   }

   private void SendMoveInstruction(GridDirection direction)
   {
      WorldManager.SetPlayerInstruction(new Instruction { 
         Direction = direction,
         Type = InstructionType.Move,
      });
   }

   public bool ProcessInput()
   {
      _lastDirection = _direction;
      _direction = Utils.VectorToDirection(Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Round());

      if (_direction == GridDirection.None) 
      {
         _directionHoldCount = 0;
      }

      if (_direction != _lastDirection)
      {
         if (_direction != GridDirection.None && _direction != Direction)
         {
            Direction = _direction;
            IdleAnimation();
         }
      }

      if (_direction != GridDirection.None &&
          _direction == _lastDirection)
      {
         if (_directionHoldCount >= _directionHoldTime)
         {
            _directionHoldCount = _directionHoldTime;
            if (WorldManager.TestTraversable(this, _direction))
            {
               SendMoveInstruction(_direction);
               return true;
            }
         } else
         {
            _directionHoldCount++;
         }
      }

      return false;
   }
}
