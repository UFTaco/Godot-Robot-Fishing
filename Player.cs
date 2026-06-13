using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	//private CharacterBody3D _player;
	private Node3D _pivotCamera;
	private SpringArm3D _springCam;

	public override void _Ready()
	{
		//_player = GetNode<CharacterBody3D>("Player");
		_pivotCamera = GetNode<Node3D>("PivotCamera");
		_springCam = _pivotCamera.GetNode<SpringArm3D>("SpringCamera");
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("player_left", "player_right", "player_forward", "player_back");
		Vector3 direction = (GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
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

	public override void _UnhandledInput(InputEvent @event)
	{
		//base._UnhandledInput(@event);
		if (Input.IsActionJustPressed("player_forward") || Input.IsActionJustPressed("player_back") || Input.IsActionJustPressed("player_left") || Input.IsActionJustPressed("player_right"))
		{

			//Rotation = new Vector3(0, _springCam.GetGlobalRotation().Y, 0);
			//RotateY(-Mathf.DegToRad(_springCam.GetGlobalRotationDegrees().Y - GetGlobalRotationDegrees().Y));

			//GD.Print("player - Y:" + Rotation.Y);

			//Rotation = new Vector3(0, _pivotCamera.GetGlobalRotation().Y, 0);
			GD.Print("playerB - Y:" + GetRotationDegrees().Y);
			RotateY(-Mathf.DegToRad(- _pivotCamera.GetRotationDegrees().Y));
			GD.Print("playerA - Y:" + GetRotationDegrees().Y);
			GD.Print(_pivotCamera.GetRotationDegrees().Y);
			_pivotCamera.SetRotationDegrees(new Vector3(0, 0, 0));
			GD.Print(_pivotCamera.GetRotationDegrees().Y);
			//GD.Print("if statement executed");

		}

		//base._UnhandledInput(@event);



		if (Input.IsActionPressed("rightClick"))
		{


			if (@event is InputEventMouseMotion mouseMotion)
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
				_pivotCamera.RotateY(mouseMotion.Relative.X * 0.01f);
				_springCam.RotateX(-mouseMotion.Relative.Y * 0.01f);
				//_springCam.RotateX(mouseMotion.Relative.Y * 0.01f);
				//_pivotCamera.SetGlobalRotation(new Vector3(0, _pivotCamera.GetGlobalRotation().Y - mouseMotion.Relative.X * 0.01f, 0));
	

				if (Input.IsActionPressed("player_forward") || Input.IsActionPressed("player_back") || Input.IsActionPressed("player_left") || Input.IsActionPressed("player_right"))
				{
					
					RotateY(-mouseMotion.Relative.X * 0.01f);
					_pivotCamera.RotateY(-mouseMotion.Relative.X * 0.01f);


				}
			}

		}
		else if (Input.IsActionJustReleased("rightClick"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}




}
