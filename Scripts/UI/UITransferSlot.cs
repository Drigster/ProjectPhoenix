using Godot;

public partial class UITransferSlot : PanelContainer
{
	private TextureRect _icon;
	private Label _amountLabel;
	private UISlot _transferedSlot;
	private bool _isTransfering;
	public bool IsTransfering => _isTransfering;

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("MarginContainer/TextureRect");
		_amountLabel = GetNode<Label>("AmountLabel");
	}

	public override void _Process(double delta)
	{
		if(_isTransfering)
		{
			Position = GetGlobalMousePosition();
		}
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("Cancel")){
			AbortTransfer();
		}
    }

    public void StartTransfer(UISlot transferedSlot)
	{
		if(GetParent().Name == "root")
		{
			CanvasLayer ui = GetNodeOrNull<CanvasLayer>("/root/Main/UI");
			if(ui != null){
				GetParent().RemoveChild(this);
				ui.AddChild(this);
			}
		}

		if(transferedSlot == null || transferedSlot.SlotData == null || transferedSlot.SlotData.ItemData == null)
		{
			return;
		}

		_transferedSlot = transferedSlot;

		_transferedSlot.Icon.Texture = null;
		_transferedSlot.AmountLabel.Text = "";

		_isTransfering = true;
		_icon.Texture = _transferedSlot.SlotData.ItemData.Icon;
		_amountLabel.Text = _transferedSlot.SlotData.Amount.ToString();
		TooltipText = _transferedSlot.SlotData.ItemData.Name + "\n\n" + _transferedSlot.SlotData.ItemData.Description;
		if(_transferedSlot.SlotData.Amount == 1 || !_transferedSlot.SlotData.ItemData.IsStackable)
		{
			_amountLabel.Visible = false;
		}
		else
		{
			_amountLabel.Visible = true;
		}
		Visible = true;
	}

	public void TransferTo(UISlot uISlot)
	{
		if(uISlot == null)
		{
			AbortTransfer();
			return;
		}

		if(uISlot.SlotData == null || uISlot.SlotData.ItemData == null)
		{
			uISlot.SlotData.Set(_transferedSlot.SlotData);
			_transferedSlot.SlotData.Set(null);
			Clear();
		}
		else{
			if(uISlot.SlotData == _transferedSlot.SlotData)
			{
				AbortTransfer();
			}
			else{
				SlotData tempSlotData = uISlot.SlotData;
				uISlot.SlotData.Set(_transferedSlot.SlotData);
				_transferedSlot.SlotData.Set(tempSlotData);
				StartTransfer(_transferedSlot);
			}
		}
		uISlot.Reload();
		_transferedSlot.Reload();
	}
 
	public void AbortTransfer(){
		if(_transferedSlot != null){
			_transferedSlot.Reload();
		}
		Clear();
	}

	private void Clear(){
		_transferedSlot = null;
		_isTransfering = false;
		_icon.Texture = null;
		_amountLabel.Text = "";
		TooltipText = "";
		Visible = false;
	}
}
