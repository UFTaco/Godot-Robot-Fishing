using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed { get; set; } = 5.0f;
	[Export]
	public float JumpVelocity { get; set; } = 4.5f;
	[Export]
	public float Gravity { get; set; } = 9.8f;
	[Export]
	public float MouseSensitivity { get; set; } = 1.00f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= Gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("player_left", "player_right", "player_forward", "player_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(-mouseMotion.Relative.X * MouseSensitivity * 0.01f);
			GetNode<Node3D>("CameraPivot").RotateX(-mouseMotion.Relative.Y * MouseSensitivity * 0.01f);
			GetNode<Node3D>("CameraPivot").RotateX(Mathf.Clamp(GetNode<Node3D>("CameraPivot").Rotation.X, Mathf.DegToRad(-55), Mathf.DegToRad(40)) - GetNode<Node3D>("CameraPivot").Rotation.X);	
		}
    }

}
