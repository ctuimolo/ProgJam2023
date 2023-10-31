using Godot;

using ProgJam2023.World;
using ProgJam2023.Dynamics;

namespace ProgJam2023.Actors.Players;

public partial class Player : GridActor
{
   [Export]
   protected FrameTimer _directionalInputTimer;

   private GridDirection _lastDirection = GridDirection.None;
   private GridDirection _direction = GridDirection.None;
   private int _directionHoldCount = 0;
   private int _directionHoldTime = 6;

   // Probably to be called in Godot scene room's _Ready function
   protected void InitPlayer()
   {
      _animationWalker.SpeedScale = 2.4f;

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
            WorldManager.TryMoveActor(this, _direction);
            return true;
         } else
         {
            _directionHoldCount++;
         }
      }

      return false;
   }
}
