using Godot;

[GlobalClass]
public partial class UIPlayerInventory : UIInventory, IUIElement
{
	[Export] private InputEventAction _inputEventAction;
	[Export] private bool _isActiveOnStart;

	public InputEventAction InputEventAction => _inputEventAction;
	public bool IsActiveOnStart => _isActiveOnStart;

	public override void _Ready()
	{
		base._Ready();
		SetInventoryData(ReferenceCenter.Player.GetNode<InventorySystem>("%Inventory"));
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
