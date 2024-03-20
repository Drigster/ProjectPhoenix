using Godot;

[GlobalClass]
public partial class Item : Resource
{
    // TODO: ItemData must never be null???
    [Export] private ItemData _itemData;
    [Export] private int _amount;

    public ItemData ItemData => _itemData;
    public int Amount => _amount;

    public Item()
    {
        _itemData = null;
        _amount = 0;
    }

    public Item(ItemData itemData, int amount)
    {
        _itemData = itemData;
        _amount = amount;
    }

    public void Set(ItemData itemData, int amount)
    {
        _itemData = itemData;
        _amount = amount;
    }

    public void Add(int amount)
    {
        if(_itemData == null)
        {
            GD.PushError("ItemData is null");
            return;
        }

        if ((_amount + amount) > _itemData.MaxStack) { 
            GD.PushError("Item Amount cant be more than MaxStack");
            return;
        }
        if (amount < 0) { 
            GD.PushError("Item Amount cant be more than MaxStack");
            return;
        }

        _amount = _amount + amount;
    }

    public void Remove(int amount)
    {
        if(_itemData == null)
        {
            GD.PushError("ItemData is null");
            return;
        }
        if ((_amount - amount) < 0) { 
            throw new System.Exception("Cant Stack Items");
        }

        _amount = _amount - amount;

        if (_amount == 0) {
            _itemData = null;
        }
    }

    public bool CanStack(Item item)
    {
        if(_itemData != item.ItemData && _itemData != null)
        {
            return false;
        }

        if((_amount + item.Amount) > item.ItemData.MaxStack)
        {
            return false;
        }
        return true;
    }

    public bool CanForceStack(Item item, out Item leftItem)
    {
        if(_itemData != item.ItemData && _itemData != null)
        {
            leftItem = null;
            return false;
        }
        if(_amount == item.ItemData.MaxStack)
        {
            leftItem = null;
            return false;
        }

        if((_amount + item.Amount) > item.ItemData.MaxStack)
        {
            leftItem = new Item(_itemData, _amount + item.Amount - item.ItemData.MaxStack);
            return true;
        }
        else
        {
            leftItem = null;
            return true;
        }
    }

    public void Stack(Item item)
    {
        if(!CanStack(item))
        {
            throw new System.Exception("Cant Stack Items");
        }

        if(_itemData == null)
        {
            _itemData = item.ItemData;
        }
        Add(item.Amount);
    }

    public bool ForceStack(Item item, out Item leftItem)
    {
        if(_itemData != item.ItemData && _itemData != null)
        {
            throw new System.Exception("Cant Stack Items, Different ItemData");
        }
        if(_amount == item.ItemData.MaxStack)
        {
            throw new System.Exception("Cant Stack Items, MaxStack Reached");
        }

        if((_amount + item.Amount) > item.ItemData.MaxStack)
        {
            if(_itemData == null)
            {
                _itemData = item.ItemData;
            }
            Add(item.ItemData.MaxStack - item.Amount);

            leftItem = new Item(_itemData, _amount + item.Amount - item.ItemData.MaxStack);
            return true;
        }
        else
        {

            if(_itemData == null)
            {
                _itemData = item.ItemData;
            }
            Add(item.Amount);

            leftItem = null;
            return true;
        }
    }
}
