using Godot;

public partial class UITransferSlot : PanelContainer
{
	private TextureRect _icon;
	private Label _amountLabel;
	private UISlot _transferedSlot;
	private ReferenceCenter _referenceCenter;
	private bool _isTransfering;
	public bool IsTransfering => _isTransfering;

    public override void _Ready()
	{
		_icon = GetNode<TextureRect>("MarginContainer/TextureRect");
		_amountLabel = GetNode<Label>("AmountLabel");
		_referenceCenter = GetNode<ReferenceCenter>("/root/ReferenceCenter");
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
		if (@event.IsActionPressed("Drop")){
			_referenceCenter.ItemsController.Spawn(_transferedSlot.Item.ItemData, _transferedSlot.Item.Amount, _referenceCenter.Player.Position);
			_transferedSlot.Set(null);
			Clear();
		}
    }

    public void StartTransfer(UISlot transferedSlot)
	{
		if(transferedSlot == null || transferedSlot.Item == null || transferedSlot.Item.ItemData == null)
		{
			return;
		}

		_transferedSlot = transferedSlot;

		_transferedSlot.Icon.Texture = null;
		_transferedSlot.AmountLabel.Text = "";

		_isTransfering = true;
		_icon.Texture = _transferedSlot.Item.ItemData.Icon;
		_amountLabel.Text = _transferedSlot.Item.Amount.ToString();
		TooltipText = _transferedSlot.Item.ItemData.Name + "\n\n" + _transferedSlot.Item.ItemData.Description;
		if(_transferedSlot.Item.Amount == 1 || !_transferedSlot.Item.ItemData.IsStackable)
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

		if(uISlot.Item == null || uISlot.Item.ItemData == null)
		{
			uISlot.Set(_transferedSlot.Item);
			_transferedSlot.Set(null);
			Clear();
		}
		else{
			if(uISlot.Item == _transferedSlot.Item)
			{
				AbortTransfer();
			}
			else{
				Item tempItem = uISlot.Item;
				uISlot.Set(_transferedSlot.Item);
				_transferedSlot.Set(tempItem);
				StartTransfer(_transferedSlot);
			}
		}
		uISlot.Reload();
		if(_transferedSlot != null){
			_transferedSlot.Reload();
		}
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
