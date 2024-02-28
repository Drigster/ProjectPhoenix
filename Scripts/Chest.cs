using Godot;
using Godot.Collections;

public partial class Chest : StaticBody2D, IInteractable
{
	private AnimationPlayer _animationPlayer;
	[Export] private int size = 12;
	InventorySystem _inventorySystem;
	private SignalCenter _signalCenter;

    public IInteractable.InteractableType Type => IInteractable.InteractableType.Storrage;

    public override void _Ready()
    {
		_inventorySystem = GetNode<InventorySystem>("%InventorySystem");
		_signalCenter = GetNode<SignalCenter>("/root/SignalCenter");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_inventorySystem.Slots = new Array<SlotData>();
		for(int i = 0; i < size; i++){
			_inventorySystem.Slots.Add(new SlotData());
		}
    }

    public void Interact(Interactor interactor, out bool interactionSuccesful)
    {
		_signalCenter.EmitSignal(nameof(_signalCenter.OpenDynamicInventory), _inventorySystem);
        _animationPlayer.Play("Open");

        interactionSuccesful = true;
    }

    public void EndInteraction()
    {
		_signalCenter.EmitSignal(nameof(_signalCenter.CloseDynamicInventory));
        _animationPlayer.Play("Close");
    }
}
