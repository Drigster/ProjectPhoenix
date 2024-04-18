using Godot;
using System;

public partial class UIRecepieIngredient : PanelContainer
{
	public void Set(AtlasTexture icon, string name, string AmountText, bool craftable) {
		GetNode<TextureRect>("MarginContainer/HBoxContainer/%Icon").Texture = icon;
		GetNode<Label>("MarginContainer/HBoxContainer/%Name").Text = name;
		GetNode<Label>("MarginContainer/HBoxContainer/%Amount").Text = AmountText;
		GetNode<ColorRect>("MarginContainer/HBoxContainer/%Color").Visible = !craftable;
	}
}
