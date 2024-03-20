using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class CraftingRecepie : Resource
{
    [Export] public Array<Item> Input { get; set; }
    [Export] public Item Output { get; set; }
}
