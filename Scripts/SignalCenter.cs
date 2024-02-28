using Godot;

public partial class SignalCenter : Node
{
	[Signal] public delegate void OpenDynamicInventoryEventHandler(InventorySystem inventory);
	[Signal] public delegate void CloseDynamicInventoryEventHandler();
}
