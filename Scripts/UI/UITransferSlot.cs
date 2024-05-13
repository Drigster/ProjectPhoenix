using System;
using Godot;

public partial class UITransferSlot : PanelContainer
{
	private TextureRect _icon;
	private Label _amountLabel;
	private ItemData _transferedData;
	private int _transferedAmount;
	public bool IsTransfering => _transferedData != null;
	public ItemData TransferedData => _transferedData;
	public int TransferedAmount => _transferedAmount;

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("MarginContainer/TextureRect");
		_amountLabel = GetNode<Label>("AmountLabel");
		ReferenceCenter.UITransferSlot = this;
	}

	public override void _Process(double delta)
	{
		if (IsTransfering)
		{
			Position = GetGlobalMousePosition();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("cancel"))
		{
			//AbortTransfer();
			AcceptEvent();
		}
	}

	public void StartTransfer(UISlot fromSlot, int amount)
	{
		_transferedData = fromSlot.Item.ItemData;
		_transferedAmount = amount;
		ReloadVisual();

		fromSlot.Item.Remove(amount);
		fromSlot.Reload();
	}

	public void StartTransfer(UISlot fromSlot)
	{
		StartTransfer(fromSlot, fromSlot.Item.Amount);
	}

	public void TransferTo(UISlot toSlot, int amount)
	{
		if (toSlot.Item.ItemData != null && toSlot.Item.ItemData != _transferedData)
		{
			throw new Exception("UITransferSlot.TransferTo | Can't transfer items of different types");
		}

		if (toSlot.Item.ItemData == null)
		{
			toSlot.Item.Set(_transferedData, amount);
		}
		else
		{
			if (toSlot.Item.Amount + amount > toSlot.Item.ItemData.MaxStack)
			{
				throw new Exception("UITransferSlot.TransferTo | Can't transfer items, target slot will be overflown");
			}

			toSlot.Item.Add(amount);
		}

		if (_transferedAmount - amount > 0)
		{
			_transferedAmount -= amount;
			ReloadVisual();
		}
		else
		{
			Clear();
		}
	}

	public void TransferTo(UISlot toSlot)
	{
		TransferTo(toSlot, _transferedAmount);
	}

	public void SwapWith(UISlot slot)
	{
		if (slot.Item.ItemData == null)
		{
			throw new Exception("UITransferSlot.SwapWith | Can't swap with empty slot");
		}
		ItemData tempData = slot.Item.ItemData;
		int tempAmount = slot.Item.Amount;

		slot.Item.Set(_transferedData, _transferedAmount);
		_transferedData = tempData;
		_transferedAmount = tempAmount;
		ReloadVisual();
	}

	private void ReloadVisual()
	{
		_icon.Texture = _transferedData.Icon;
		_amountLabel.Text = _transferedAmount.ToString();
		if (_transferedAmount == 1 || !_transferedData.IsStackable)
		{
			_amountLabel.Visible = false;
		}
		else
		{
			_amountLabel.Visible = true;
		}
		Visible = true;
	}

	private void Clear()
	{
		_transferedData = null;
		_transferedAmount = 0;
		_icon.Texture = null;
		_amountLabel.Text = "";
		TooltipText = "";
		Visible = false;
	}
}
