using Godot;

public partial class Player : CharacterBody3D
{
	public float MaxSpeed = 20F;
	public float Acceleration = 200F;
	public int FallAcceleration = 75;

	private float _mouseSensitivity = 0.3F;
	private Camera3D _camera;

	// TODO: Move to separate Physics class
	private float _friction = 6F;

	private Vector3 _velocity;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseMotion mouseMotionEvent)
		{
			RotateY(Mathf.DegToRad(-mouseMotionEvent.Relative.X * _mouseSensitivity));

			var change = Mathf.DegToRad(-mouseMotionEvent.Relative.Y * _mouseSensitivity);
			var futureCameraAngleV = change + _camera.Rotation.X;

			// We should correct change in order not to overshoot [-90, 90] degrees.
			// If everything is in limits, the expression simplifies to change = change.
			// If not, change is corrected by the delta between overshot value and limits
			change = change - (futureCameraAngleV - Mathf.Clamp(futureCameraAngleV,
				-Mathf.Pi / 2,
				Mathf.Pi / 2
			));

			_camera.RotateX(change);
		}
	}

	public override void _PhysicsProcess(double doubleDelta)
	{
		float delta = (float)doubleDelta;

		var direction = Vector3.Zero;

		Vector3 rightAxis = GlobalTransform.Basis.X;
		Vector3 forwardAxis = GlobalTransform.Basis.Z;

		// TODO: Implement camera and then get its angle
		if (Input.IsActionPressed("move_forward"))
		{
			direction -= forwardAxis;
		}
		if (Input.IsActionPressed("move_backward"))
		{
			direction += forwardAxis;
		}
		if (Input.IsActionPressed("move_right"))
		{
			direction += rightAxis;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction -= rightAxis;
		}

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
		}

		_velocity = Velocity;

		applyFriction(delta);

		var addVelocity = direction * Acceleration * delta;

		_velocity.X += addVelocity.X;
		_velocity.Z += addVelocity.Z;

		_velocity.LimitLength(MaxSpeed);

        // TODO: Implement jumps
        _velocity.Y = 0;

		Velocity = _velocity;
		MoveAndSlide(); //since 4.0 MoveAndSlide now works with object properties (e.g. Velocity)
	}

	private void applyFriction(float delta)
	{
		float speed = _velocity.Length();

		if (speed < 1)
		{
			_velocity.X = 0;
			_velocity.Z = 0;
			return;
		}

		float multiplier = 1 - _friction * delta;
		_velocity.X *= multiplier;
		_velocity.Z *= multiplier;
	}
}
