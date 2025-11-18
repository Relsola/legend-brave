using Godot;

public partial class Player : CharacterBody2D
{
    private Variant gravity = ProjectSettings.GetSetting("physics/2d/default_gravity");
    private const float RunSpeed = 200.0f;
    private const float JumpVelocity = -300.0f;

    private Sprite2D _sprite2D;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        // 控制台打印消息

        var velocity = Velocity;
        var direction = Input.GetAxis("move_left", "move_right");

        velocity.Y += (float)gravity * (float)delta;
        velocity.X = direction * RunSpeed;

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            velocity.Y = JumpVelocity;
        }

        if (IsOnFloor())
        {
            if (Mathf.IsZeroApprox(direction))
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


        Velocity = velocity;
        MoveAndSlide();
    }
}
