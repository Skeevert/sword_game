using Godot;
using System;

public class Player : KinematicBody
{
	public float MaxSpeed = 20F;
	public float Acceleration = 200F;
	public int FallAcceleration = 75;

	private float _mouseSensitivity = 0.3F;
	private Camera _camera;
	
	// TODO: Move to separate Physics class
	private float _friction = 6F;
	
	private Vector3 _velocity = Vector3.Zero;
	
	public override void _Ready()
	{
		_camera = GetNode<Camera>("Camera");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseMotion mouseMotionEvent)
		{
			RotateY(Mathf.Deg2Rad(-mouseMotionEvent.Relative.x * _mouseSensitivity));
			
			var change = Mathf.Deg2Rad(-mouseMotionEvent.Relative.y * _mouseSensitivity);
			var futureCameraAngleV = change + _camera.Rotation.x;

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
	
	public override void _PhysicsProcess(float delta)
	{
		var direction = Vector3.Zero;
		
		Vector3 rightAxis = GlobalTransform.basis.x;
		Vector3 forwardAxis = GlobalTransform.basis.z;
		

		applyFriction(delta);
		
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
		
		var addVelocity = direction * Acceleration *delta;
		
		_velocity.x += addVelocity.x;
		_velocity.z += addVelocity.z;
		
		limitVelocity();
	
		// TODO: Implement jumps
		_velocity.y = 0;
		
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
	}
	
	private void applyFriction(float delta)
	{
		float speed = _velocity.Length();
		
		if (speed < 1)
		{
			_velocity.x = 0;
			_velocity.z = 0;
			return;
		}
		
		float multiplier = 1 - _friction * delta;
		_velocity.x *= multiplier;
		_velocity.z *= multiplier;
	}
	
	private void limitVelocity()
	{
		_velocity = _velocity.LimitLength(MaxSpeed);
	}
}
