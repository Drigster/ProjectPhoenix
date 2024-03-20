using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventorySystem : Node
{
	[Signal] public delegate void OnInventoryChangedEventHandler();

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
    
    public bool CanAddItem(Item item)
    {
        return CanAddItems(item.ItemData, item.Amount);
    }

    public bool CanAddItems(ItemData itemData, int amount)
    {
        CanAddItemsCount(itemData, out int amountToAdd);
        return amountToAdd >= amount;
    }

    public bool CanAddItemsCount(ItemData itemData, out int amount)
    {
        amount = 0;
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].ItemData == itemData || Items[i].ItemData == null)
            {
                amount += itemData.MaxStack - Items[i].Amount;
            }
        }
        return amount > 0;
    }

    public void AddItem(Item item){
        AddItems(item.ItemData, item.Amount);
    }

    public void AddItems(ItemData itemData, int amount)
    {
        if(!CanAddItems(itemData, amount))
        {
            throw new Exception("InventorySystem.AddItems: Dont have enough space.");
        }

        Array<Item> items = GetItems(itemData);
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].Amount + amount <= items[i].ItemData.MaxStack)
            {
                items[i].Add(amount);
                EmitSignal(nameof(OnInventoryChanged));
                return;
            }
            else
            {
                amount -= items[i].ItemData.MaxStack - items[i].Amount;
                items[i].Add(items[i].ItemData.MaxStack - items[i].Amount);
                EmitSignal(nameof(OnInventoryChanged));
            }
        }
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].ItemData == null)
            {
                Items[i] = new Item(itemData, amount);
                EmitSignal(nameof(OnInventoryChanged));
                return;
            }
        }

        GD.PushError("InventorySystem.AddItems: Could not add all items.");
    }

    public int CountItems(ItemData itemData){
        int count = 0;
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].ItemData == itemData)
            {
                count += Items[i].Amount;
            }
        }
        return count;
    }

    public void RemoveItem(Item item)
    {
        RemoveItems(item.ItemData, item.Amount);
    }

    public void RemoveItems(ItemData itemData, int amount)
    {
        if(CountItems(itemData) < amount)
        {
            throw new Exception("InventorySystem.RemoveItems: Dont have enough items.");
        }

        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].ItemData == itemData)
            {
                if(Items[i].Amount >= amount)
                {
                    Items[i].Remove(amount);
                    EmitSignal(nameof(OnInventoryChanged));
                    return;
                }
                else
                {
                    amount -= Items[i].Amount;
                    Items[i] = new Item(null, 0);
                    EmitSignal(nameof(OnInventoryChanged));
                }
            }
        }

        throw new Exception("InventorySystem.RemoveItems: Could not remove all items.");
    }

    private Array<Item> GetItems(ItemData itemData){
        Array<Item> items = new Array<Item>();
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].ItemData == itemData)
            {
                items.Add(Items[i]);
            }
        }

        return items;
    }
}
