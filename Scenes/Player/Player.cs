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
		SouthWest
	}

	private Dictionary<int, MapDirections> _dictMapDirections = new Dictionary<int, MapDirections>()
	{
		{ 0, MapDirections.North },
		{ 90, MapDirections.East },
		{ 180, MapDirections.South },
		{ 270, MapDirections.West },
	};

	private Dictionary<MapDirections, int> _dictCoordinates = new Dictionary<MapDirections, int>()
	{
		{ MapDirections.North, 0 },
		{ MapDirections.South, 180 },
		{ MapDirections.East, 90 },
		{ MapDirections.West, 270 },
	};

	private Dictionary<MapDirections, Vector2> _dictVectors = new Dictionary<MapDirections, Vector2>()
	{
		{ MapDirections.North, new Vector2(0, -1) },
		{ MapDirections.South, new Vector2(0, 1) },
		{ MapDirections.East, new Vector2(1, 0) },
		{ MapDirections.West, new Vector2(-1, 0) },
		{ MapDirections.NorthEast, new Vector2(1, -1) },
		{ MapDirections.NorthWest, new Vector2(-1, -1) },
		{ MapDirections.SouthEast, new Vector2(1, 1) },
		{ MapDirections.SouthWest, new Vector2(-1, 1) },
	};

	[Export] private float _moveSpeed = 100.0f;
	[Export] private float _rotationSpeed = 10.0f;
	[Export] private Label _debugLabel;
	[Export] private Node2D _sprites;

	private Vector2 _currentVelocity;
	private Vector2 _directionToMoveIn;

	public override void _Ready()
	{
		_directionToMoveIn = _dictVectors[MapDirections.North];
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		HandleInput();

		Velocity = _currentVelocity;
		MoveAndSlide();
	}

	private void CalculateNewDirection(int rotation)
	{
		Vector2 v = new Vector2(0, 0);
		float offset = 0.0f;

		if (rotation > 0 && rotation < 90)
		{
			v = _dictVectors[MapDirections.NorthEast];
			offset = rotation / 90;
		}
		else if (rotation > 90 && rotation < 180)
		{
			v = _dictVectors[MapDirections.SouthEast];
			offset = (rotation - 90) / 90;
		}
		else if (rotation > 180 && rotation < 270)
		{
			v = _dictVectors[MapDirections.SouthWest];
			offset = (rotation - 180) / 90;
		}
		else if (rotation > 270 && rotation < 360)
		{
			v = _dictVectors[MapDirections.NorthWest];
			offset = (rotation - 270) / 90;
		}
		_directionToMoveIn = v + v * offset;
	}

	private void RotateTank(float rotation)
	{
		_sprites.RotationDegrees += rotation;

		int r = (int)_sprites.RotationDegrees;
		r %= 360;
		r = r >= 0 ? r : 360 + r;

		if (_dictMapDirections.ContainsKey(r))
		{
			_directionToMoveIn = _dictVectors[_dictMapDirections[r]];
		}
		else
		{
			CalculateNewDirection(r);
		}
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
			GD.Print(_directionToMoveIn);
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
