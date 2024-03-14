using Godot;

[GlobalClass]
public partial class UIDynamicInventory : UIInventory
{
	private SignalCenter _signalCenter;
	private UIElement _parent;

	public override void _Ready()
	{
		base._Ready();
		_signalCenter = GetNode<SignalCenter>("/root/SignalCenter");
		_parent = GetParent<UIElement>();

		_signalCenter.OpenDynamicInventory += (InventorySystem inventorySystem) => {
			SetInventoryData(inventorySystem);
			_parent.Visible = true;
		};
		_signalCenter.CloseDynamicInventory += () => {
			SetInventoryData(null);
			_parent.Visible = false;
		};
	}
}
