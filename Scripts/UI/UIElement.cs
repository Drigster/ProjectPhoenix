using Godot;
using System;

[GlobalClass]
public partial class UIElement : Control
{
	[Export]
	public InputEventAction inputEventAction;
	[Export]
	public Boolean isActiveOnStart = false;
}
