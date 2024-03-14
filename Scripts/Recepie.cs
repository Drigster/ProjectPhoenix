using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Recepie : Resource
{
    [Export] public Array<Item> Input { get; set; }
    [Export] public Array<Item> Output { get; set; }
    public enum RecepieType { 
        Crafting,
        Smelting
    }
    [Export] public RecepieType Type { get; set; }
}
