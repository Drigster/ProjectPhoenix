using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventorySystem : Node
{
    [Export] public Array<SlotData> Slots { get; set; }

    public override void _EnterTree()
    {
        if(Slots == null)
        {
            Slots = new Array<SlotData>();
        }
        for(int i = 0; i < Slots.Count; i++)
        {
            if(Slots[i] == null)
            {
                Slots[i] = new SlotData();
            }
        }
    }
}
