using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private float _speed = 100.0f;
	private AnimationTree _animationTree;
	public override void _Ready()
	{
		_animationTree = GetNode<AnimationTree>("AnimationTree");
		ReferenceCenter.Player = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");

		Velocity = direction * _speed;

		_animationTree.Set("parameters/conditions/isMoving", direction.Length() > 0.01f);
		_animationTree.Set("parameters/conditions/isIdle", direction.Length() <= 0.01f);
		_animationTree.Set("parameters/Walk/blend_position", direction);

		MoveAndSlide();
	}
}
