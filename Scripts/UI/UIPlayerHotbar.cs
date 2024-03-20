using Godot;

[GlobalClass]
public partial class UIPlayerHotbar : UIInventory
{
	private int _selectedSlot = -1;

	public override void _Ready()
	{
		base._Ready();
		ReferenceCenter referenceCenter = GetNode<ReferenceCenter>("/root/ReferenceCenter");
		SetInventoryData(referenceCenter.Player.GetNode<InventorySystem>("%Hotbar"));
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
