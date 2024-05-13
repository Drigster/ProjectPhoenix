using Godot;

[GlobalClass]
public partial class UISlot : Panel
{
	[Signal] public delegate void OnSlotClickedEventHandler(InputEvent inputEvent);
	private TextureRect _icon;
	private Label _amountLabel;
	[Export] private Item _item;

	public TextureRect Icon => _icon;
	public Label AmountLabel => _amountLabel;

	public Item Item => _item;

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("MarginContainer/TextureRect");
		_amountLabel = GetNode<Label>("MarginContainer/AmountLabel");
	}

	public void Set(Item item)
	{
		_item = item;

		Reload();
		item.Changed += Reload;
	}

	public void Reload()
	{
		if (_item != null && _item.ItemData != null)
		{
			_icon.Texture = _item.ItemData.Icon;
			_amountLabel.Text = _item.Amount.ToString();
			TooltipText = _item.ItemData.Name + "\n\n" + _item.ItemData.Description;

			if (_item.Amount == 1 || !_item.ItemData.IsStackable)
			{
				_amountLabel.Visible = false;
			}
			else
			{
				_amountLabel.Visible = true;
			}
		}
		else
		{
			_icon.Texture = null;
			_amountLabel.Text = "";
			_amountLabel.Visible = false;
			TooltipText = "";
		}
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			EmitSignal(nameof(OnSlotClicked), @event);
		}
	}
}
