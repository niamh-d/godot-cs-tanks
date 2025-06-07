using System.Collections.Generic;
using Godot;
public partial class Player : CharacterBody2D
{

	private enum MapDirections
	{
		North,
		South,
		East,
		West,
		NorthEast,
		NorthWest,
		SouthEast,
		SouthWest,
	}

	private Dictionary<int, MapDirections> _dictRotations = new Dictionary<int, MapDirections>()
	{
		{ 0, MapDirections.North },
		{ 90, MapDirections.East },
		{ 180, MapDirections.South },
		{ 270, MapDirections.West },
		{ 45, MapDirections.NorthEast },
		{ 135, MapDirections.SouthEast },
		{ 225, MapDirections.SouthWest },
		{ 315, MapDirections.NorthWest}
	};

	private Dictionary<MapDirections, Vector2> _dictDirections = new Dictionary<MapDirections, Vector2>()
	{
		{ MapDirections.North, new Vector2(0, -1) },
		{ MapDirections.South, new Vector2(0, 1) },
		{ MapDirections.East, new Vector2(1, 0) },
		{ MapDirections.West, new Vector2(-1, 0) },
		{ MapDirections.NorthWest, new Vector2(-1, -1) },
		{ MapDirections.NorthEast, new Vector2(1, -1) },
		{ MapDirections.SouthWest, new Vector2(-1, 1) },
		{ MapDirections.SouthEast, new Vector2(1, 1) }
	};

	[Export] private float _moveSpeed = 100.0f;
	[Export] private float _rotationSpeed = 45.0f;
	[Export] private Label _debugLabel;
	[Export] private Node2D _sprites;

	private Vector2 _currentVelocity;
	private Vector2 _directionToMoveIn;

	public override void _Ready()
	{
		_directionToMoveIn = _dictDirections[MapDirections.North];
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		HandleInput();

		Velocity = _currentVelocity;
		Debug();
		MoveAndSlide();
	}

	private void Debug()
	{
		_debugLabel.Text = $"Velocity: (X: {Velocity.X}, Y: {Velocity.Y})";
		GD.Print($"Velocity: (X: {Velocity.X}, Y: {Velocity.Y})");
	}

	private void RotateTank(float rotation)
	{
		_sprites.RotationDegrees += rotation;

		int r = (int)_sprites.RotationDegrees;
		r = r >= 0 ? r : 360 + r;
		MapDirections dir = _dictRotations[r];
		_directionToMoveIn = _dictDirections[dir];
	}

	private void HandleInput()
	{

		if (Input.IsActionJustPressed("rotate_right"))
		{
			RotateTank(_rotationSpeed);
		}

		if (Input.IsActionJustPressed("rotate_left"))
		{
			RotateTank(-_rotationSpeed);
		}

		if (Input.IsActionPressed("move_forward"))
		{
			_currentVelocity = _directionToMoveIn * _moveSpeed;
		}
		else if (Input.IsActionPressed("move_backward"))
		{
			_currentVelocity = -_directionToMoveIn * _moveSpeed;
		}
		else
		{
			_currentVelocity = Vector2.Zero;
		}
	}
}
