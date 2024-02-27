using Godot;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export] private ItemData _itemData;
    private int _amount;

    public ItemData ItemData => _itemData;
    [Export] public int Amount { 
        get => _amount;
        set {
            if (value < 1){
                GD.PushError(_itemData.Name + " can't have 0 or negative amount. Changing amount to 1. Was: " + value);
                _amount = 1;
            }
            else if (value > _itemData.MaxStack){
                GD.PushError(_itemData.Name + " can't have amount more than " + _itemData.MaxStack + ". Changing amount to " + _itemData.MaxStack + ". Was: " + value);
                _amount = _itemData.MaxStack;
            }
            else if (!_itemData.IsStackable && value > 1){
                GD.PushError(_itemData.Name + " is not stackable. Changing amount to 1. Was: " + value);
                _amount = 1;
            }
            else{
                _amount = value;
            }
        }
    }

    public void Set(SlotData slotData)
    {
        if(slotData == null)
        {
            _itemData = null;
            _amount = 0;
            return;
        }
        _itemData = slotData.ItemData;
        _amount = slotData.Amount;
    }
}
