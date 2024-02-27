using Godot;
using System;

public partial class UISlot : PanelContainer
{
	private TextureRect _icon;
	private Label _amountLabel;
	[Export] private SlotData _slotData;
	private UITransferSlot _transferSlot;

	public TextureRect Icon => _icon;
	public Label AmountLabel => _amountLabel;

	public SlotData SlotData => _slotData;

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("MarginContainer/TextureRect");
		_amountLabel = GetNode<Label>("AmountLabel");
		_transferSlot = GetNode<UITransferSlot>("/root/UITransferSlot");
	}

	public void Init(SlotData slotData)
	{
		_slotData = slotData;
		Reload();
	}

	public void Reload(){
		if(_slotData != null && _slotData.ItemData != null){
			_icon.Texture = _slotData.ItemData.Icon;
			_amountLabel.Text = _slotData.Amount.ToString();
			TooltipText = _slotData.ItemData.Name + "\n\n" + _slotData.ItemData.Description;

			if(_slotData.Amount == 1 || !_slotData.ItemData.IsStackable)
			{
				_amountLabel.Visible = false;
			}
			else
			{
				_amountLabel.Visible = true;
			}
		}
		else{
			_icon.Texture = null;
			_amountLabel.Text = "";
			_amountLabel.Visible = false;
			TooltipText = "";
		}
	}

	public void OnGuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseButton)
		{
			if((mouseButton.ButtonIndex == MouseButton.Left || 
			   mouseButton.ButtonIndex == MouseButton.Right) &&
			   mouseButton.Pressed)
			{
				if(_transferSlot.IsTransfering)
				{
					_transferSlot.TransferTo(this);
				}
				else
				{
					_transferSlot.StartTransfer(this);
				}
			}
		}
	}
}
