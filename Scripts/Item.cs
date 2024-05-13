using Godot;

[GlobalClass]
public partial class Item : Resource
{
	[Export] protected ItemData _itemData;
	[Export] protected int _amount;

	public ItemData ItemData => _itemData;
	public int Amount => _amount;

	public Item()
	{
		_itemData = null;
		_amount = 0;
	}

	public Item(ItemData itemData, int amount)
	{
		Set(itemData, amount);
	}

	public void Set(ItemData itemData, int amount)
	{
		if (itemData != null && amount > itemData.MaxStack)
		{
			GD.PushError("Item Amount cant be more than MaxStack. Setting to MaxStack(" + itemData.MaxStack + ")!");
			amount = itemData.MaxStack;
		}
		else if (amount < 0)
		{
			GD.PushError("Item Amount cant be less than 1. Setting to 1!");
			amount = 1;
		}
		_itemData = itemData;
		_amount = amount;

		EmitChanged();
	}

	public void Add(int amount)
	{
		if (_itemData == null)
		{
			GD.PushError("ItemData is null");
			return;
		}

		if ((_amount + amount) > _itemData.MaxStack)
		{
			GD.PushError("Item Amount cant be more than MaxStack");
			return;
		}
		if (amount < 0)
		{
			GD.PushError("Item Amount cant be less than 0");
			return;
		}

		_amount += amount;

		EmitChanged();
	}

	public void Remove(int amount)
	{
		if (_itemData == null)
		{
			GD.PushError("ItemData is null");
			return;
		}
		if ((_amount - amount) < 0)
		{
			throw new System.Exception("Cant Stack Items");
		}

		_amount -= amount;

		if (_amount == 0)
		{
			_itemData = null;
		}

		EmitChanged();
	}
}
