using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventorySystemGroup : Node, IInventorySystem
{
	[Signal] public delegate void OnInventoryChangedEventHandler();
	[Export] private Array<InventorySystem> _inventorySystems;

	public override void _Ready()
	{
		_inventorySystems = new Array<InventorySystem>();

		Array<Node> children = GetChildren();
		foreach (Node child in children)
		{
			if (child is InventorySystem inventorySystem)
			{
				_inventorySystems.Add(inventorySystem);
				inventorySystem.OnInventoryChanged += () => EmitSignal(nameof(OnInventoryChanged));
			}
		}
	}

	public void AddItems(Item item)
	{
		AddItems(item.ItemData, item.Amount);
	}

	public void AddItems(ItemData itemData, int amount)
	{
		if (CountAvailableItemSpace(itemData) < amount)
		{
			throw new Exception("InventorySystemGroup.AddItems: Dont have enough space.");
		}

		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			int itemsToAdd = inventorySystem.CountAvailableItemSpace(itemData);
			if (amount > itemsToAdd)
			{
				inventorySystem.AddItems(itemData, itemsToAdd);
				amount -= itemsToAdd;
			}
			else
			{
				inventorySystem.AddItems(itemData, amount);
				amount = 0;
				break;
			}
		}

		if (amount > 0)
		{
			throw new Exception("InventorySystemGroup.AddItems: Error adding all items.");
		}
	}

	public void RemoveItems(Item item)
	{
		RemoveItems(item.ItemData, item.Amount);
	}

	public void RemoveItems(ItemData itemData, int amount)
	{
		if (CountItems(itemData) < amount)
		{
			throw new Exception("InventorySystemGroup.RemoveItems: Dont have enough items.");
		}

		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			int itemsToRemove = inventorySystem.CountAvailableItemSpace(itemData);
			if (amount > itemsToRemove)
			{
				inventorySystem.RemoveItems(itemData, itemsToRemove);
				amount -= itemsToRemove;
			}
			else
			{
				inventorySystem.RemoveItems(itemData, amount);
				amount = 0;
				break;
			}
		}

		if (amount > 0)
		{
			throw new Exception("InventorySystemGroup.RemoveItems: Could not remove all items.");
		}
	}

	public int CountItems(ItemData itemData)
	{
		int count = 0;
		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			count += inventorySystem.CountItems(itemData);
		}

		return count;
	}

	public int CountEmptySlots()
	{
		int count = 0;
		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			count += inventorySystem.CountEmptySlots();
		}

		return count;
	}

	public int CountNotFullSlotSpaces(ItemData itemData)
	{
		int count = 0;
		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			count += inventorySystem.CountNotFullSlotSpaces(itemData);
		}

		return count;
	}

	public int CountAvailableItemSpace(ItemData itemData)
	{
		int count = 0;
		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			count += inventorySystem.CountAvailableItemSpace(itemData);
		}

		return count;
	}

	public InventorySystem GetInventory()
	{
		Array<Item> items = new Array<Item>();

		foreach (InventorySystem inventorySystem in _inventorySystems)
		{
			items += inventorySystem.Items;
		}

		return new InventorySystem(items);
	}

	// private Array<Item> GetItems(ItemData itemData)
	// {
	//     Array<Item> _items = new Array<Item>();
	//     foreach (Item item in _items)
	//     {
	//         if(item.ItemData == itemData)
	//         {
	//             _items.Add(item);
	//         }
	//     }

	//     return _items;
	// }

	// private Array<Item> GetNotFullItems(ItemData itemData)
	// {
	//     Array<Item> _items = new Array<Item>();
	//     foreach (Item item in _items)
	//     {
	//         if(item.ItemData == itemData && item.Amount < item.ItemData.MaxStack)
	//         {
	//             _items.Add(item);
	//         }
	//     }

	//     return _items;
	// }

	// public Array<Item> GetEmptySlots()
	// {
	//     Array<Item> _items = new Array<Item>();
	//     foreach (Item item in _items)
	//     {
	//         if(item.ItemData == null)
	//         {
	//             _items.Add(item);
	//         }
	//     }

	//     return _items;
	// }
}
