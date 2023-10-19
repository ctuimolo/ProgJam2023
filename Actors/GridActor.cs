using Godot;
using ProgJam2023.World;
using System;

namespace ProgJam2023.Actors;

public partial class GridActor : Node2D
{
   public enum ActorState
   {
      Idle,
      Busy,
      NeedsUpdate,
   }

   [Export]
   public ActorState State;

   [Export]
   protected AnimationPlayer _animationPlayer;

   [Export]
   protected AnimationPlayer _animationWalker;

   [Export]
   protected int _stepSpeed = 18; // Speed is framecount of total animation, not velocity (smaller=faster)

   public Direction Direction = Direction.Down;

   public Vector2I CurrentCell = Vector2I.Zero;
   public Vector2I NextCell = Vector2I.Zero;

   public void IdleAnimation()
   {
      if (State == ActorState.Idle)
      {
         switch (Direction)
         {
            case Direction.Up:
               _animationPlayer.Play("idle_up");
               break;
            case Direction.Down:
               _animationPlayer.Play("idle_down");
               break;
            case Direction.Left:
               _animationPlayer.Play("idle_left");
               break;
            case Direction.Right:
               _animationPlayer.Play("idle_right");
               break;
         }
         _animationWalker.Play("idle");
      }
   }

   public void StepAnimation(Direction direction)
   {
      Direction = direction;

      switch (Direction)
      {
         case Direction.Up:
            _animationPlayer.Play("walk_up");
            _animationWalker.Play("walk_up");
            break;
         case Direction.Down:
            _animationPlayer.Play("walk_down");
            _animationWalker.Play("walk_down");
            break;
         case Direction.Left:
            _animationPlayer.Play("walk_left");
            _animationWalker.Play("walk_left");
            break;
         case Direction.Right:
            _animationPlayer.Play("walk_right");
            _animationWalker.Play("walk_right");
            break;
      }
      _animationWalker.Queue("idle");
   }
}
