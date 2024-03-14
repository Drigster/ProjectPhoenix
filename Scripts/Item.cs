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

    public void Add(int amount)
    {
        if(_itemData == null)
        {
            GD.PushError("ItemData is null");
            return;
        }
        if ((_amount + amount) > _itemData.MaxStack) { GD.PushError("Item Amount cant be more than MaxStack"); }

        _amount = _amount + amount;
    }

    public void Remove(int amount)
    {
        if(_itemData == null)
        {
            GD.PushError("ItemData is null");
            return;
        }
        if ((_amount - amount) < 0) { GD.PushError("Item Amount cant be less than 0"); }

        _amount = _amount - amount;
    }
}
