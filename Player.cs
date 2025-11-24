using Godot;

public partial class Player : CharacterBody2D
{
	private Variant gravity = ProjectSettings.GetSetting("physics/2d/default_gravity");
	private const float RunSpeed = 200.0f;
	private const float FLOOR_ACCELERATION = RunSpeed / 0.2f;
	private const float JUMP_ACCELERATION = RunSpeed / 0.02f;
	private const float JumpVelocity = -300.0f;

	private Sprite2D _sprite2D;
	private AnimationPlayer _animationPlayer;
	private Timer _coyoteTimer;
	private Timer _jumpRequestTimer;

	public override void _Ready()
	{
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_coyoteTimer = GetNode<Timer>("CoyoteTimer");
		_jumpRequestTimer = GetNode<Timer>("JumpRequestTimer");

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		var velocity = Velocity;

		if (@event.IsActionPressed("jump"))
		{
			_jumpRequestTimer.Start();
		}

		if (@event.IsActionReleased("jump") && velocity.Y < JumpVelocity / 2)
		{
			velocity.Y = JumpVelocity / 2;
			Velocity = velocity;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		var direction = Input.GetAxis("move_left", "move_right");

		var acceleration = IsOnFloor() ? FLOOR_ACCELERATION : JUMP_ACCELERATION;
		velocity.Y += (float)gravity * (float)delta;
		velocity.X = (float)Mathf.MoveToward(velocity.X, direction * RunSpeed, acceleration * delta);

		var can_jump = IsOnFloor() || _coyoteTimer.TimeLeft > 0.0;
		var should_jump = can_jump && _jumpRequestTimer.TimeLeft > 0.0;

		if (should_jump)
		{
			velocity.Y = JumpVelocity;
			_coyoteTimer.Stop();
			_jumpRequestTimer.Stop();
		}

		if (IsOnFloor())
		{
			if (Mathf.IsZeroApprox(direction) && Mathf.IsZeroApprox(velocity.X))
			{
				_animationPlayer.Play("idle");
			}
			else
			{
				_animationPlayer.Play("running");
				_sprite2D.FlipH = direction < 0;
			}
		}
		else
		{
			_animationPlayer.Play("jump");
		}


		if (!Mathf.IsZeroApprox(direction))
		{
			_sprite2D.FlipH = direction < 0;
		}

		var was_on_floor = IsOnFloor();

		Velocity = velocity;
		MoveAndSlide();

		if (!IsOnFloor())
		{
			if (was_on_floor && !should_jump) _coyoteTimer.Start();
			else _coyoteTimer.Stop();
		}
	}
}
