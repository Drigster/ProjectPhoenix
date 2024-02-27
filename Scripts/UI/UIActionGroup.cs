using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UIActionGroup : Resource
{
    [Export] public InputEventAction Action;
    [Export] public Array<UIElement> Elements;
    [Export] public bool IsOpen = false;
}
