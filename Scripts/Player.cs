using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private float Speed = 100.0f;
	private AnimationTree _animationTree;
	public override void _Ready(){
		_animationTree = GetNode<AnimationTree>("AnimationTree");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");

		Velocity = direction * Speed;

		_animationTree.Set("parameters/conditions/isMoving", direction.Length() > 0.01f);
		_animationTree.Set("parameters/conditions/isIdle", direction.Length() <= 0.01f);
		_animationTree.Set("parameters/Walk/blend_position", direction);

		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event.IsActionPressed("inventory_toggle"))
		{
		}
	}
}
