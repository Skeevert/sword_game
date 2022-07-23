using Godot;
using System;

public class Player : KinematicBody
{
	public int Speed = 14;
	public int FallAcceleration = 75;

	private float _mouseSensitivity = 0.3F;
	private Camera _camera;
	
	private Vector3 _velocity = Vector3.Zero;
	
	public override void _Ready()
	{
		_camera = GetNode<Camera>("Camera");
		Input.SetMouseMode(Input.MouseMode.Captured);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseMotion mouseMotionEvent)
		{
//			RotateY(Mathf.Deg2Rad(-mouseMotionEvent.Relative.x * _mouseSensitivity));
			
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
		
		_velocity.x = direction.x * Speed;
		_velocity.z = direction.z * Speed;
		
		// TODO: Implement jumps
		_velocity.y = 0;
		
		_velocity = MoveAndSlide(_velocity, Vector3.Up);
	}
}
