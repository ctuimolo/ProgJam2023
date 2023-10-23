using Godot;

using ProgJam2023.World;
using ProgJam2023.Dynamics;

namespace ProgJam2023.Actors.Player;

public partial class Player : GridActor
{
   [Export]
   protected FrameTimer _directionalInputTimer;

   private GridDirection _lastDirection = GridDirection.None;
   private GridDirection _direction     = GridDirection.None;
   private int _directionHoldCount  = 0;
   private int _directionHoldTime   = 5;

   // Probably to be called in Godot scene room's _Ready function
   protected void InitPlayer()
   {
      _animationWalker.SpeedScale = 1.7f;

      WorldManager.SetCurrentPlayer(this);
   }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      InitPlayer();
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override async void _Process(double delta)
   {
      ProcessInput();
   }

   private void ProcessInput()
   {
      if (WorldManager.CurrentState == WorldManager.State.Open)
      {
         _lastDirection = _direction;
         _direction = Utils.VectorToDirection(Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Round());

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
               _directionHoldCount = 0;
            } else
            {
              _directionHoldCount++;
            }
         }

         if (_direction == GridDirection.None)
         {
            _directionHoldCount = 0;
         }
      }
   }
}
