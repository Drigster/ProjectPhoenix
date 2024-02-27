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
		PopulateItemGrid(inventoryData);
	}

	public void Reload()
	{
		PopulateItemGrid(_inventorySystem);
	}

	public void PopulateItemGrid(InventorySystem inventoryData)
	{
		foreach (Node child in _slotsContainer.GetChildren())
		{
			child.QueueFree();
		}

		for (int i = 0; i < _inventorySystem.Slots.Count; i++)		
		{
			UISlot slotScene = (UISlot)SlotScene.Instantiate();
			slotScene.Name = "Slot " + i;
			_slotsContainer.AddChild(slotScene);

			// slotScene.OnSlotClicked += inventoryData.OnSlotClicked;

			if(_inventorySystem.Slots[i] != null)
			{
				slotScene.Init(_inventorySystem.Slots[i]);
			}
		}
	}
}
