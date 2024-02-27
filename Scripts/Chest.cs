using Godot;

public partial class Chest : StaticBody2D, IInteractable
{
	AnimationPlayer animationPlayer;

    public IInteractable.InteractableType Type => IInteractable.InteractableType.Storrage;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Interact(Interactor interactor, out bool interactionSuccesful)
    {
		animationPlayer.Play("Open");

		interactionSuccesful = true;
    }

    public void EndInteraction()
    {
		animationPlayer.Play("Close");
    }
}
