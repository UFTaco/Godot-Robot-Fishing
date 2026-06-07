using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// How fast the player moves in meters per second.
	[Export]
	public int Speed { get; set; } = 50;
	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int FallAcceleration { get; set; } = 75;
	[Export]
	public int JumpImpulse { get; set; } = 50;

	[Export]
	public int Sensitivity { get; set; } = 100;
	private Vector3 _targetVelocity = Vector3.Zero;


	public override void _PhysicsProcess(double delta)
	{

		var direction = Vector3.Zero;

		// Moving the character
		Velocity = _targetVelocity;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		if (IsOnFloor() && Input.IsActionJustPressed("Jump"))
		{
			_targetVelocity.Y = JumpImpulse;
		}
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			// Setting the basis property will affect the rotation of the node.
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}
		
		// Ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Vertical velocity
		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			GD.Print("Mouse motion: " + mouseMotion.Relative);
			//GetNode<Node3D>("Pivot").RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * Sensitivity * (float)GetProcessDeltaTime()));
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * Sensitivity * (float)GetProcessDeltaTime()));
			
		}
	}
}
