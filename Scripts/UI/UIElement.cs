using Godot;
using System;

[GlobalClass]
public partial class UIElement : Control
{
	[Export]
	public InputEventAction inputEventAction;
	[Export]
	public bool isActiveOnStart = false;

	public void Close(){
		Visible = false;
	}

	public void Open(){
		Visible = true;
	}
}
