using Godot;

public partial class UIRecepieIngredient : PanelContainer
{
	public void Set(AtlasTexture icon, string name, string amountText, bool craftable)
	{
		GetNode<TextureRect>("MarginContainer/HBoxContainer/%Icon").Texture = icon;
		GetNode<Label>("MarginContainer/HBoxContainer/%Name").Text = name;
		GetNode<Label>("MarginContainer/HBoxContainer/%Amount").Text = amountText;
		GetNode<ColorRect>("MarginContainer/HBoxContainer/%Color").Visible = !craftable;
	}
}
