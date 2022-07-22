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
			this.RotateY(Mathf.Deg2Rad(-mouseMotionEvent.Relative.x * _mouseSensitivity));
			_camera.RotateX(Mathf.Deg2Rad(-mouseMotionEvent.Relative.y * _mouseSensitivity));
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		var direction = Vector3.Zero;
		
		
		// TODO: Implement camera and then get its angle
		if (Input.IsActionPressed("move_forward"))
		{
			
		}
		if (Input.IsActionPressed("move_backward"))
		{
			
		}
		if (Input.IsActionPressed("move_right"))
		{
			
		}
		if (Input.IsActionPressed("move_left"))
		{
			
		}
	}
}
