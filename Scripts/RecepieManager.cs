using Godot;
using Godot.Collections;
using System;

public partial class RecepieManager : Node
{
	[Export] private Array<CraftingRecepie> _craftingRecepies;

	public Array<CraftingRecepie> CraftingRecepies => _craftingRecepies;
}
