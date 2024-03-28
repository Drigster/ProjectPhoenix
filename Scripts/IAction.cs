using Godot;

public interface IAction
{
    public void Action(Node2D caller, Vector2 target);
}