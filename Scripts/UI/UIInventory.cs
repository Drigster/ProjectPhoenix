using Godot;

[GlobalClass]
public partial class UIInventory : PanelContainer
{
	private PackedScene SlotScene = GD.Load<PackedScene>("res://Scenes/UI/UISlot.tscn");
	private GridContainer _slotsContainer;
	private InventorySystem _inventorySystem;

	public override void _Ready()
	{
		_slotsContainer = GetNode<GridContainer>("MarginContainer/ItemGrid");
	}

	public void SetInventoryData(InventorySystem inventoryData)
	{
		_inventorySystem = inventoryData;
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

			// slotScene.OnSlotClicked += inventoryData.OnSlotClicked;

			if(_inventorySystem.Items[i] != null)
			{
				slotScene.Set(_inventorySystem.Items[i]);
			}
		}
	}
}
