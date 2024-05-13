using Godot;

public interface IAction
{
	public void Action(Item self, Node2D caller, Vector2 target);
}