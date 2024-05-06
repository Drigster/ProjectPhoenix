using System;
using Godot;

[GlobalClass]
public partial class UIInventory : PanelContainer
{
	private PackedScene SlotScene = GD.Load<PackedScene>("res://Scenes/UI/UISlot.tscn");
	private GridContainer _slotsContainer;
	private InventorySystem _inventorySystem;
	private UITransferSlot _transferSlot;

	public override void _Ready()
	{
		_slotsContainer = GetNode<GridContainer>("MarginContainer/ItemGrid");
		_transferSlot = ReferenceCenter.UITransferSlot;
	}

	public void SetInventoryData(InventorySystem inventoryData)
	{
		if(inventoryData != null)
		{
			_inventorySystem = inventoryData;
			_inventorySystem.OnInventoryChanged += Reload;
		}
		else
		{
			_inventorySystem = null;
		}
		Reload();
	}

	public void Reload()
	{
		foreach (Node child in _slotsContainer.GetChildren())
		{
			child.QueueFree();
		}

		if(_inventorySystem == null)
		{
			return;
		}

		for (int i = 0; i < _inventorySystem.Items.Count; i++)
		{
			UISlot slotScene = SlotScene.Instantiate<UISlot>();
			slotScene.Name = "Slot " + i;
			_slotsContainer.AddChild(slotScene);

			if(_inventorySystem.Items[i] != null)
			{
				slotScene.Set(_inventorySystem.Items[i]);
			}

			slotScene.OnSlotClicked += (inputEvent) => {
				if(inputEvent is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
				{
					if(_transferSlot.IsTransfering)
					{
						if(slotScene.Item.ItemData == _transferSlot.TransferedData)
						{
							if(slotScene.Item.Amount + _transferSlot.TransferedAmount > slotScene.Item.ItemData.MaxStack)
							{
								_transferSlot.SwapWith(slotScene);
							}
							else{
								_transferSlot.TransferTo(slotScene);
							}
						}
						else if(slotScene.Item.ItemData == null){
							_transferSlot.TransferTo(slotScene);
						}
						else{
							_transferSlot.SwapWith(slotScene);
						}
					}
					else
					{
						_transferSlot.StartTransfer(slotScene);
					}
					_inventorySystem.EmitSignal(nameof(_inventorySystem.OnInventoryChanged));
				}
				else if(inputEvent is InputEventMouseButton mouseButton2 && mouseButton2.Pressed && mouseButton2.ButtonIndex == MouseButton.Right)
				{
					if(_transferSlot.IsTransfering)
					{
						if(slotScene.Item.ItemData == _transferSlot.TransferedData)
						{
							if(slotScene.Item.Amount + _transferSlot.TransferedAmount >= slotScene.Item.ItemData.MaxStack)
							{
								_transferSlot.SwapWith(slotScene);
							}
							else{
								_transferSlot.TransferTo(slotScene, 1);
							}
							_inventorySystem.EmitSignal(nameof(_inventorySystem.OnInventoryChanged));
						}
						else if(slotScene.Item.ItemData == null){
							_transferSlot.TransferTo(slotScene, 1);
							_inventorySystem.EmitSignal(nameof(_inventorySystem.OnInventoryChanged));
						}
						else{
							_transferSlot.SwapWith(slotScene);
							_inventorySystem.EmitSignal(nameof(_inventorySystem.OnInventoryChanged));
						}
					}
					else{
						int splitAmount = 1;
						if(slotScene.Item.Amount > 1)
						{
							splitAmount = (int)Math.Floor(slotScene.Item.Amount / 2.0);
						}
						_transferSlot.StartTransfer(slotScene, splitAmount);
						_inventorySystem.EmitSignal(nameof(_inventorySystem.OnInventoryChanged));
					}
				}
			};
		}
	}

	public UISlot GetSlot(int index)
	{
		if(index >= _slotsContainer.GetChildCount()){
			throw new IndexOutOfRangeException("UIInventory.GetSlot: Index out of range");
		}

		return _slotsContainer.GetChild<UISlot>(index);
	}
}
