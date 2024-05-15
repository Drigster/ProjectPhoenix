using Godot;

[GlobalClass]
public partial class UIPlayerHotbar : UIInventory, IUIElement
{
	private int _selectedSlot = -1;
	private Player _player;
	[Export] private InputEventAction _inputEventAction;
	[Export] private bool _isActiveOnStart;

	public InputEventAction InputEventAction => _inputEventAction;
	public bool IsActiveOnStart => _isActiveOnStart;

	public override void _Ready()
	{
		base._Ready();
		_player = ReferenceCenter.Player;
		SetInventoryData(_player.FindChild("Hotbar") as InventorySystem);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (_selectedSlot != -1)
		{
			Item item = GetSlot(_selectedSlot).Item;
			if (item.ItemData is IProcessAction processAction)
			{
				processAction.ProcessAction();
			}

			// TOGO: Remove shit code
			GetSlot(_selectedSlot).ThemeTypeVariation = "SelectedSlotPanel";
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("action1"))
		{
			if (_selectedSlot != -1)
			{
				Item item = GetSlot(_selectedSlot).Item;
				if (item.ItemData is IAction action)
				{
					action.Action(item, _player, _player.GetGlobalMousePosition());
				}
			}
		}

		if (@event.IsActionPressed("action2"))
		{
			if (_selectedSlot != -1)
			{
				Item item = GetSlot(_selectedSlot).Item;
				if (item.ItemData is ISecondaryAction secondaryAction)
				{
					secondaryAction.SecondaryAction(item, _player, _player.GetGlobalMousePosition());
				}
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("select_slot_1"))
		{
			SelectSlot(0);
			AcceptEvent();
		}
		else if (@event.IsActionPressed("select_slot_2"))
		{
			SelectSlot(1);
			AcceptEvent();
		}
		else if (@event.IsActionPressed("select_slot_3"))
		{
			SelectSlot(2);
			AcceptEvent();
		}
		else if (@event.IsActionPressed("select_slot_4"))
		{
			SelectSlot(3);
			AcceptEvent();
		}
		else if (@event.IsActionPressed("select_slot_5"))
		{
			SelectSlot(4);
			AcceptEvent();
		}
		else if (@event.IsActionPressed("select_slot_6"))
		{
			SelectSlot(5);
			AcceptEvent();
		}
	}

	public void SelectSlot(int slotIndex)
	{
		if (_selectedSlot != -1)
		{
			GetSlot(_selectedSlot).ThemeTypeVariation = "SlotPanel";
		}

		Item item = GetSlot(_selectedSlot).Item;
		if (item.ItemData is IProcessAction processAction)
		{
			processAction.EndProcessAction();
		}

		if (slotIndex == _selectedSlot)
		{
			_selectedSlot = -1;
			return;
		}

		_selectedSlot = slotIndex;
		item = GetSlot(_selectedSlot).Item;
		if (item.ItemData is IProcessAction processAction2)
		{
			processAction2.StartProcessAction();
		}
		GetSlot(_selectedSlot).ThemeTypeVariation = "SelectedSlotPanel";
	}

	public void Close()
	{
		Visible = false;
	}

	public void Open()
	{
		Visible = true;
	}
}
