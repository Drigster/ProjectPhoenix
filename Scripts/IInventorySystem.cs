using Godot;

public interface IInventorySystem
{
	[Signal] public delegate void OnInventoryChangedEventHandler();

	public void AddItems(Item item);
	public void AddItems(ItemData itemData, int amount);
	public void RemoveItems(Item item);
	public void RemoveItems(ItemData itemData, int amount);
	public int CountItems(ItemData itemData);
	public int CountEmptySlots();
	public int CountNotFullSlotSpaces(ItemData itemData);
	public int CountAvailableItemSpace(ItemData itemData);
	public InventorySystem GetInventory();

	// private Array<Item> GetItems(ItemData itemData);

	// private Array<Item> GetNotFullItems(ItemData itemData);

	// public Array<Item> GetEmptySlots();
}
