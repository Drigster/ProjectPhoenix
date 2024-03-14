using Godot;

[GlobalClass]
public partial class UIPlayerHotbar : UIInventory
{
	public override void _Ready()
	{
		base._Ready();
		ReferenceCenter referenceCenter = GetNode<ReferenceCenter>("/root/ReferenceCenter");
		SetInventoryData(referenceCenter.Player.GetNode<InventorySystem>("%Hotbar"));
	}
}
