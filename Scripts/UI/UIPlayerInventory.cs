using Godot;
using System;

public partial class UIPlayerInventory : UIInventory
{
	[Export] private Player _playerData;

	public override void _Ready()
	{
		base._Ready();
		SetInventoryData(_playerData.GetNode<InventorySystem>("%Inventory"));
	}
}
