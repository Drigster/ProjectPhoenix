using Godot;
using Godot.Collections;

public partial class MinableResource : StaticBody2D, IInteractable
{
	private AnimationPlayer _animationPlayer;
	[Export] private ItemData _resourceType;
    public IInteractable.InteractableType Type => IInteractable.InteractableType.Resource;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Interact(Interactor interactor, out bool interactionSuccesful)
    {
        _animationPlayer.Play("Hit");

        interactionSuccesful = true;
    }

    public void EndInteraction() {}
}
