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

    public Vector2I CurrentCell = new Vector2I(0, 0);

    public void SingleStep(Direction direction)
    {
        Direction = direction;

        switch (Direction)
        {
            case Direction.Up:
                _animationWalker.Play("walk_up");
                _animationWalker.Queue("idle");
                break;
            case Direction.Down:
                State = ActorState.Busy;
                _animationPlayer.Play("walk_down");
                _animationWalker.Play("walk_down");
                _animationWalker.Queue("idle");
                break;
            case Direction.Left:
                _animationWalker.Play("walk_left");
                _animationWalker.Queue("idle");
                break;
            case Direction.Right:
                _animationWalker.Play("walk_right");
                _animationWalker.Queue("idle");
                break;
        }
    }
}
