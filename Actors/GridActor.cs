using Godot;

using ProgJam2023.Rooms;
using ProgJam2023.World;

namespace ProgJam2023.Actors;

public enum GridActorType
{
   Player,
   Enemy,
   Object,
   Door,
}

public partial class GridActor : Node2D
{
   [Export]
   public GridActorType Type { get; set; }

   public enum ActorCollisionType
   {
      None,
      Solid,
   }

   public enum ActorState
   {
      Idle,
      Busy,
      NeedsUpdate,
   }

   [Export]
   public ActorState State;

   [Export]
   public ActorCollisionType CollisionType;

   [Export]
   protected AnimationPlayer _animationPlayer;

   [Export]
   protected AnimationPlayer _animationWalker;

   [Export]
   protected int _stepSpeed = 18; // Speed is framecount of total animation, not velocity (smaller=faster)

   public GridDirection Direction = GridDirection.Down;

   public Cell CurrentCell    = null;
   public GridDirection LastStep;

   Label _debugText;
   PackedScene _debugTextScene = ResourceLoader.Load<PackedScene>("res://Debug/DebugText.tscn");

   public override void _Ready()
   {
      _debugText = _debugTextScene.Instantiate<Label>();
      _debugText.Visible = false;
      AddChild(_debugText);
   }

   public override void _Process(double delta)
   {
      _debugText.Visible = WorldManager.DrawDebugText;
      if(_debugText.Visible)
      {
         _debugText.Text = 
            $"{Name}\n({CurrentCell?.X},{CurrentCell?.Y})";
      }
   }

   public void IdleAnimation()
   {
      if (State == ActorState.Idle)
      {
         switch (Direction)
         {
            case GridDirection.Up:
               _animationPlayer.Play("idle_up");
               break;
            case GridDirection.Down:
               _animationPlayer.Play("idle_down");
               break;
            case GridDirection.Left:
               _animationPlayer.Play("idle_left");
               break;
            case GridDirection.Right:
               _animationPlayer.Play("idle_right");
               break;
         }
         _animationWalker.Play("idle");
      }
   }

   public void StepAnimation(GridDirection direction)
   {
      Direction = direction;

      switch (Direction)
      {
         case GridDirection.Up:
            _animationPlayer.Play("walk_up");
            _animationWalker.Play("walk_up");
            break;
         case GridDirection.Down:
            _animationPlayer.Play("walk_down");
            _animationWalker.Play("walk_down");
            break;
         case GridDirection.Left:
            _animationPlayer.Play("walk_left");
            _animationWalker.Play("walk_left");
            break;
         case GridDirection.Right:
            _animationPlayer.Play("walk_right");
            _animationWalker.Play("walk_right");
            break;
      }
      _animationWalker.Queue("idle");
   }
}
