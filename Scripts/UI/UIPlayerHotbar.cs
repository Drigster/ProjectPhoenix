using Godot;

[GlobalClass]
public partial class UIPlayerHotbar : UIInventory
{
	private int _selectedSlot = -1;
	private Player player;

	public override void _Ready()
	{
		base._Ready();
		ReferenceCenter referenceCenter = GetNode<ReferenceCenter>("/root/ReferenceCenter");
		player = referenceCenter.Player;
		SetInventoryData(player.GetNode<InventorySystem>("%Hotbar"));
	}

    public override void _Input(InputEvent @event)
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

		if (@event.IsActionPressed("action1"))
		{
			if(_selectedSlot != -1){
				Item item = GetSlot(_selectedSlot).Item;
				if((item.ItemData as Tool) != null){
					(item.ItemData as Tool).Action(player, player.GetGlobalMousePosition());
				}
			}
		}

		if (@event.IsActionPressed("action2"))
		{
			if(_selectedSlot != -1){
				Item item = GetSlot(_selectedSlot).Item;
				if((item.ItemData as ISecondaryAction) != null){
					(item.ItemData as ISecondaryAction).SecondaryAction();
				}
			}
		}
    }

	public void SelectSlot(int slotIndex)
	{
		if(_selectedSlot != -1){
			GetSlot(_selectedSlot).ThemeTypeVariation = "SlotPanel";
		}
		if(slotIndex == _selectedSlot){
			_selectedSlot = -1;
			return;
		}
		
		_selectedSlot = slotIndex;
		GetSlot(_selectedSlot).ThemeTypeVariation = "SelectedSlotPanel";
	}
}
