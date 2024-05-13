using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class UIActionGroup : Resource
{
	public InputEventAction Action;
	public List<IUIElement> Elements;
	public bool IsOpen = false;
}
