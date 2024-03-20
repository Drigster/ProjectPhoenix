using Godot;
using System;

public partial class UICraftingRecepie : TextureRect
{
	[Signal] public delegate void OnRecepieClickedEventHandler();
	[Export] private CraftingRecepie _craftingRecepie;

	public override void _Ready()
	{
		base._Ready();
		GuiInput += OnGuiInput;
	}

    public UICraftingRecepie(CraftingRecepie craftingRecepie)
	{
		_craftingRecepie = craftingRecepie;
		Texture = craftingRecepie.Output.ItemData.Icon;
	}

    private void OnGuiInput(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left){
        	EmitSignal(nameof(OnRecepieClicked));
		}
    }
}
