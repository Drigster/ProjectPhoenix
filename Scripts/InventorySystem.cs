using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventorySystem : Node
{
    [Export] public Array<Item> Items { get; set; }

    public override void _EnterTree()
    {
        if(Items == null)
        {
            Items = new Array<Item>();
        }
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i] == null)
            {
                Items[i] = new Item(null, 0);
            }
        }
    }
}
