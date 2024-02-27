using Godot;
using System;

public partial class UIPlayerHotbar : UIInventory
{
	[Export] private Player _playerData;

	public override void _Ready()
	{
		base._Ready();
		SetInventoryData(_playerData.GetNode<InventorySystem>("%Hotbar"));
	}
}
