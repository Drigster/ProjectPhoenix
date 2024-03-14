using Godot;

[GlobalClass]
public partial class GroundItem : RigidBody2D
{
	[Export] private Sprite2D _icon;
	[Export] private Label _amountLabel;
	private Item _item;

	private ItemsController _itemsController;

	public Item Item => _item;

	public override void _Ready()
	{
		_itemsController = GetNode<ReferenceCenter>("/root/ReferenceCenter").ItemsController;
	}

	public void SetItem(Item item)
	{
		_item = item;
		_icon.Texture = _item.ItemData.Icon;
		_amountLabel.Text = _item.Amount.ToString();
	}

	public void OnBodyEntered(Node2D body){
		if(body == this){
			return;
		}
        if(body is not GroundItem){
            return;
        }
		
		if(TryStack(body as GroundItem)){
			body.QueueFree();
		}
    }

    private bool TryStack(GroundItem groundItem)
    {
        if (groundItem.GetInstanceId() < GetInstanceId())
		{
			return false;
		}

		int tempAmount = _item.Amount + groundItem.Item.Amount - _item.ItemData.MaxStack;
		if (tempAmount > 0)
		{
			_item.Add(_item.Amount - _item.ItemData.MaxStack);

			_itemsController.Spawn(_item.ItemData, tempAmount, Position);
		}
		else
		{
			_item.Add(groundItem.Item.Amount);
		}
		_amountLabel.Text = _item.Amount.ToString();
		return true;
    }
}
