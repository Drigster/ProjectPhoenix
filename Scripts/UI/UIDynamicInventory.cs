using Godot;

[GlobalClass]
public partial class UIDynamicInventory : UIInventory, IUIElement
{
	private SignalCenter _signalCenter;
	[Export] private InputEventAction _inputEventAction;
	[Export] private bool _isActiveOnStart;

	public InputEventAction InputEventAction => _inputEventAction;
	public bool IsActiveOnStart => _isActiveOnStart;

	public override void _Ready()
	{
		base._Ready();
		_signalCenter = GetNode<SignalCenter>("/root/SignalCenter");

		_signalCenter.OpenDynamicInventory += (InventorySystem inventorySystem) =>
		{
			SetInventoryData(inventorySystem);
			Open();
		};
		_signalCenter.CloseDynamicInventory += () =>
		{
			SetInventoryData(null);
			Close();
		};
	}

	public void Close()
	{
		Visible = false;
	}

	public void Open()
	{
		Visible = true;
	}
}
