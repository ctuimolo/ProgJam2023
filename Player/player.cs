using Godot;
using System;

public partial class player : CharacterBody2D
{
	[Export]
	AnimationPlayer _spriteAnimator = null;

	[Export]
	Sprite2D _sprite = null;

	int jumpFrames = 30;
	int currentJumpFrame = 0;
	bool doJump = false;

	public const float Speed = 120.0f;
	public const float JumpVelocity = -330.0f;

	Vector2 direction;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (doJump)
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
		if (Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right")) 
		{
			_sprite.FlipH = true;
			if (IsOnFloor())
			{
                _spriteAnimator?.Play("walking");
            }
        }

        if (Input.IsActionPressed("ui_right") & !Input.IsActionPressed("ui_left"))
        {
            _sprite.FlipH = false;
            if (IsOnFloor())
            {
                _spriteAnimator?.Play("walking");
            }
        }

		if ((!Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right")) ||
            (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_right")))
		{
			if (IsOnFloor())
			{
				_spriteAnimator?.Play("idle");
			}
		}

		if (IsOnFloor())
		{
			currentJumpFrame = 0;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("ui_accept") && currentJumpFrame < jumpFrames)
		{
			doJump = true;
		}

		if (Input.IsActionPressed("ui_accept") && doJump)
		{
			currentJumpFrame++;
		}

		if (!Input.IsActionPressed("ui_accept") || currentJumpFrame >= jumpFrames)
		{
			doJump = false;
		}

		if (!IsOnFloor())
		{
			if (Velocity.Y < 0 )
			{
                _spriteAnimator?.Play("rising");
            } else
			{
                _spriteAnimator?.Play("falling");
            }
        }
    }
}
