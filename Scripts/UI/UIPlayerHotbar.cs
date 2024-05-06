using Godot;

[GlobalClass]
public partial class UIPlayerHotbar : UIInventory
{
	private int _selectedSlot = -1;
	private Player player;

	public override void _Ready()
	{
		base._Ready();
		player = ReferenceCenter.Player;
		SetInventoryData(player.GetNode<InventorySystem>("%InventorySystemGroup/%Hotbar"));
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if(_selectedSlot != -1){
			Item item = GetSlot(_selectedSlot).Item;
			if(item.ItemData is IProcessAction processAction){
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
			if(_selectedSlot != -1){
				Item item = GetSlot(_selectedSlot).Item;
				if(item.ItemData is IAction action){
					action.Action(item, player, player.GetGlobalMousePosition());
				}
			}
		}

		if (@event.IsActionPressed("action2"))
		{
			if(_selectedSlot != -1){
				Item item = GetSlot(_selectedSlot).Item;
				if(item.ItemData is ISecondaryAction secondaryAction){
					secondaryAction.SecondaryAction(item, player, player.GetGlobalMousePosition());
				}
			}
		}
    }

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("select_slot_1"))
		{
			SelectSlot(0);
		}
		else if (@event.IsActionPressed("select_slot_2"))
		{
			SelectSlot(1);
		}
		else if (@event.IsActionPressed("select_slot_3"))
		{
			SelectSlot(2);
		}
		else if (@event.IsActionPressed("select_slot_4"))
		{
			SelectSlot(3);
		}
		else if (@event.IsActionPressed("select_slot_5"))
		{
			SelectSlot(4);
		}
		else if (@event.IsActionPressed("select_slot_6"))
		{
			SelectSlot(5);
		}
    }

	public void SelectSlot(int slotIndex)
	{
		if(_selectedSlot != -1){
			GetSlot(_selectedSlot).ThemeTypeVariation = "SlotPanel";
		}
		
		Item item = GetSlot(_selectedSlot).Item;
		if(item.ItemData is IProcessAction processAction){
			processAction.EndProcessAction();
		}

		if(slotIndex == _selectedSlot){
			_selectedSlot = -1;
			return;
		}

		_selectedSlot = slotIndex;
		item = GetSlot(_selectedSlot).Item;
		if(item.ItemData is IProcessAction processAction2){
			processAction2.StartProcessAction();
		}
		GetSlot(_selectedSlot).ThemeTypeVariation = "SelectedSlotPanel";
	}
}
