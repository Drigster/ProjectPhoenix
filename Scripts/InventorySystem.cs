using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventorySystem : Node, IInventorySystem
{
	[Signal] public delegate void OnInventoryChangedEventHandler();

	[Export] private Array<Item> _items;

	public Array<Item> Items => _items;

	public InventorySystem()
	{
		_items = new Array<Item>();
	}

	public InventorySystem(Array<Item> items)
	{
		_items = items;
	}

	public override void _EnterTree()
	{
		if (_items == null)
		{
			_items = new Array<Item>();
		}
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i] == null)
			{
				_items[i] = new Item(null, 0);
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
			throw new Exception("InventorySystem.AddItems: Dont have enough space.");
		}

		Array<Item> _notFullItems = GetNotFullItems(itemData);

		foreach (Item item in _notFullItems)
		{
			int amountToAdd = item.ItemData.MaxStack - item.Amount;
			if (amount > amountToAdd)
			{
				item.Add(amountToAdd);
				amount -= amountToAdd;
			}
			else
			{
				item.Add(amount);
				amount = 0;
				break;
			}
		}

		if (amount > 0)
		{
			Array<Item> _emptySlots = GetEmptySlots();

			foreach (Item item in _emptySlots)
			{
				int amountToAdd = itemData.MaxStack;
				if (amount > amountToAdd)
				{
					item.Set(itemData, amountToAdd);
					amount -= amountToAdd;
				}
				else
				{
					item.Set(itemData, amount);
					amount = 0;
					break;
				}
			}
		}

		EmitSignal(nameof(OnInventoryChanged));

		if (amount > 0)
		{
			throw new Exception("InventorySystem.AddItems: Error adding all items.");
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
			throw new Exception("InventorySystem.RemoveItems: Dont have enough items.");
		}

		Array<Item> items = GetItems(itemData);
		foreach (Item item in items)
		{
			int amountToRemove = item.Amount;
			if (amount > amountToRemove)
			{
				item.Remove(amountToRemove);
				amount -= amountToRemove;
			}
			else
			{
				item.Remove(amount);
				amount = 0;
				break;
			}
		}

		EmitSignal(nameof(OnInventoryChanged));

		if (amount > 0)
		{
			throw new Exception("InventorySystem.RemoveItems: Could not remove all items.");
		}
	}

	public int CountItems(ItemData itemData)
	{
		int count = 0;
		foreach (Item item in _items)
		{
			if (item.ItemData == itemData)
			{
				count += item.Amount;
			}
		}
		return count;
	}

	public int CountEmptySlots()
	{
		int count = 0;
		foreach (Item item in _items)
		{
			if (item.ItemData == null)
			{
				count++;
			}
		}

		return count;
	}

	public int CountNotFullSlotSpaces(ItemData itemData)
	{
		int count = 0;
		foreach (Item item in GetNotFullItems(itemData))
		{
			count += item.ItemData.MaxStack - item.Amount;
		}

		return count;
	}

	public int CountAvailableItemSpace(ItemData itemData)
	{
		return (CountEmptySlots() * itemData.MaxStack) + CountNotFullSlotSpaces(itemData);
	}

	private Array<Item> GetItems(ItemData itemData)
	{
		Array<Item> items = new Array<Item>();
		foreach (Item item in _items)
		{
			if (item.ItemData == itemData)
			{
				items.Add(item);
			}
		}

		return items;
	}

	private Array<Item> GetNotFullItems(ItemData itemData)
	{
		Array<Item> items = new Array<Item>();
		foreach (Item item in _items)
		{
			if (item.ItemData == itemData && item.Amount < item.ItemData.MaxStack)
			{
				items.Add(item);
			}
		}

		return items;
	}

	public Array<Item> GetEmptySlots()
	{
		Array<Item> items = new Array<Item>();
		foreach (Item item in _items)
		{
			if (item.ItemData == null)
			{
				items.Add(item);
			}
		}

		return items;
	}

	public InventorySystem GetInventory()
	{
		return this;
	}
}
