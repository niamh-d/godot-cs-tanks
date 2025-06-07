using Godot;

public partial class Player : CharacterBody2D
{

	[Export] private float _moveSpeed = 100.0f;

	private Vector2 currentVelocity;


	public override void _Ready()
	{
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		handleInput();

		Velocity = currentVelocity;
		MoveAndSlide();
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("move_left", "move_right", "move_forward", "move_backward") * _moveSpeed;
	}
}
