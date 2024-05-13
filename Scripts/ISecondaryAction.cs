using Godot;

public interface ISecondaryAction
{
	public void SecondaryAction(Item self, Node2D caller, Vector2 interationPosition);
}