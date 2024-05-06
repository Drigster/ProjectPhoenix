using Godot;

[GlobalClass]
public partial class UIPlayerInventory : UIInventory
{
	public override void _Ready()
	{
		base._Ready();
		SetInventoryData(ReferenceCenter.Player.GetNode<InventorySystem>("%Inventory"));
	}
}
